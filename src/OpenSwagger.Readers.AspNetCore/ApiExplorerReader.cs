using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Readers.Interface;
using Newtonsoft.Json;
using OpenSwagger.Readers.AspNetCore.Extensions;
using OpenSwagger.Readers.AspNetCore.Generator;

namespace OpenSwagger.Readers.AspNetCore
{
    public sealed class ApiExplorerReader : IOpenApiReader<IApiDescriptionGroupCollectionProvider, OpenApiDiagnostic>
    {
        private readonly ApiExplorerReaderSettings settings;

        public ApiExplorerReader(ApiExplorerReaderSettings settings)
        {
            this.settings = settings;
        }

        public OpenApiDocument Read(IApiDescriptionGroupCollectionProvider input, out OpenApiDiagnostic diagnostic)
        {
            diagnostic = new OpenApiDiagnostic();
            var schemaRegistry = new SchemaRegistry(new JsonSerializerSettings());

            var apiDescriptions = input.ApiDescriptionGroups.Items
                .SelectMany(group => group.Items);

            var paths = new OpenApiPaths();
            foreach (var group in apiDescriptions.GroupBy(apiDesc => apiDesc.RelativePathSansQueryString()))
            {
                paths.Add("/" + group.Key, CreatePathItem(group, schemaRegistry));
            }

            var openApiDocument = new OpenApiDocument
            {
                Paths = paths,
            };

            var filterContext = new DocumentFilterContext(input.ApiDescriptionGroups, schemaRegistry);
            foreach (var filter in this.settings.DocumentFilters)
            {
                filter.Apply(openApiDocument, filterContext);
            }

            return openApiDocument;
        }

        private OpenApiPathItem CreatePathItem(IEnumerable<ApiDescription> apiDescriptions, ISchemaRegistry schemaRegistry)
        {
            var pathItem = new OpenApiPathItem();

            // Group further by http method
            var perMethodGrouping = apiDescriptions.GroupBy(apiDesc => apiDesc.HttpMethod);

            foreach (var group in perMethodGrouping)
            {
                var httpMethod = group.Key;

                if (httpMethod == null)
                {
                    throw new NotSupportedException(
                        $"Ambiguous HTTP method for action - {group.First().ActionDescriptor.DisplayName}. " +
                        "Actions require an explicit HttpMethod binding for Swagger");
                }

                if (group.Count() > 1)
                {
                    throw new NotSupportedException(
                        $"HTTP method \"{httpMethod}\" & path \"{group.First().RelativePathSansQueryString()}\" overloaded " +
                        $"by actions - {string.Join(",", group.Select(apiDesc => apiDesc.ActionDescriptor.DisplayName))}. " +
                        "Actions require unique method/path combination for Swagger");
                }

                var apiDescription = group.Single();

                switch (httpMethod)
                {
                    case "GET":
                        pathItem.Operations[OperationType.Get] = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "PUT":
                        pathItem.Operations[OperationType.Put] = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "POST":
                        pathItem.Operations[OperationType.Post] = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "DELETE":
                        pathItem.Operations[OperationType.Delete] = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "OPTIONS":
                        pathItem.Operations[OperationType.Options] = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "HEAD":
                        pathItem.Operations[OperationType.Head] = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "PATCH":
                        pathItem.Operations[OperationType.Patch] = CreateOperation(apiDescription, schemaRegistry);
                        break;
                    case "TRACE":
                        pathItem.Operations[OperationType.Trace] = CreateOperation(apiDescription, schemaRegistry);
                        break;
                }
            }

            return pathItem;
        }

        private OpenApiOperation CreateOperation(ApiDescription apiDescription, ISchemaRegistry schemaRegistry)
        {
            var parameters = apiDescription.ParameterDescriptions
                .Where(IsParameter)
                .Where(paramDesc => paramDesc.Source.IsFromRequest && !paramDesc.IsPartOfCancellationToken())
                .Select(paramDesc => CreateParameter(apiDescription, paramDesc, schemaRegistry))
                .ToList();

            var requestBody = CreateRequestBody(apiDescription, schemaRegistry);

            var responses = new OpenApiResponses();
            foreach (var apiResponseType in apiDescription
                .SupportedResponseTypes
                .DefaultIfEmpty(new ApiResponseType { StatusCode = 200 }))
            {
                responses.Add(apiResponseType.StatusCode.ToString(), CreateResponse(apiResponseType, schemaRegistry));
            }

            var operation = new OpenApiOperation
            {
                Tags = new[] { new OpenApiTag { Name = apiDescription.ControllerName() } },
                OperationId = apiDescription.FriendlyId(),
                Parameters = parameters.Any() ? parameters : null, // parameters can be null but not empty
                RequestBody = requestBody,
                Responses = responses,
                Deprecated = apiDescription.IsObsolete()
            };

            var filterContext = new OperationFilterContext(apiDescription, schemaRegistry);
            foreach (var filter in this.settings.OperationFilters)
            {
                filter.Apply(operation, filterContext);
            }

            return operation;
        }

        private OpenApiRequestBody CreateRequestBody(ApiDescription apiDescription, ISchemaRegistry schemaRegistry)
        {
            var bodyParameter = apiDescription.ParameterDescriptions.FirstOrDefault(IsRequestBody);
            if (bodyParameter == null) return null;

            // Workaround for IFormFile
            if (bodyParameter.Type == typeof(IFormFile))
            {
                return new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    [bodyParameter.Name] = new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    }
                                }
                            }
                        }
                    }
                };
            }

            var content = apiDescription.SupportedRequestFormats.ToDictionary(
                apiResponseType => apiResponseType.MediaType,
                apiResponseType => new OpenApiMediaType
                {
                    Schema = (bodyParameter.Type != null && bodyParameter.Type != typeof(void))
                            ? schemaRegistry.GetOrRegister(bodyParameter.Type)
                            : null
                }
            );

            var requestBody = new OpenApiRequestBody
            {
                Content = content
            };

            return requestBody;
        }

        private OpenApiParameter CreateParameter(
            ApiDescription apiDescription,
            ApiParameterDescription paramDescription,
            ISchemaRegistry schemaRegistry)
        {
            ParameterLocation GetParameterLocation(ApiParameterDescription param)
            {
                if (param.Source == BindingSource.Header)
                    return ParameterLocation.Header;
                else if (param.Source == BindingSource.Path)
                    return ParameterLocation.Path;
                else if (param.Source == BindingSource.Query)
                    return ParameterLocation.Query;

                // None of the above, default to "query"
                // Wanted to default to "body" for PUT/POST but ApiExplorer flattens out complex params into multiple
                // params for ALL non-bound params regardless of HttpMethod. So "query" across the board makes most sense
                return ParameterLocation.Query;
            }

            ParameterStyle GetStyle(ParameterLocation parameterLocation)
            {
                switch (parameterLocation)
                {
                    case ParameterLocation.Query:
                    case ParameterLocation.Cookie:
                        return ParameterStyle.Form;
                    case ParameterLocation.Path:
                    case ParameterLocation.Header:
                        return ParameterStyle.Simple;
                    default:
                        return ParameterStyle.Simple;
                }
            }

            var location = GetParameterLocation(paramDescription);

            var parameter = new OpenApiParameter
            {
                Name = paramDescription.Name,
                In = location,
                Required = (location == ParameterLocation.Path) || paramDescription.IsRequired(),
            };

            var schema = (paramDescription.Type == null) ? null : schemaRegistry.GetOrRegister(paramDescription.Type);
            if (schema != null && (schema.Type == "object" || schema.Type == "array"))
            {
                var style = GetStyle(location);
                parameter.Style = GetStyle(location);
                parameter.Explode = style == ParameterStyle.Form;
            }

            parameter.Schema = schema;

            return parameter;
        }

        private OpenApiResponse CreateResponse(ApiResponseType apiResponseType, ISchemaRegistry schemaRegistry)
        {
            var description = ResponseDescriptionMap
                .FirstOrDefault((entry) => Regex.IsMatch(apiResponseType.StatusCode.ToString(), entry.Key))
                .Value;

            var content = apiResponseType.ApiResponseFormats.ToDictionary(
                resp => resp.MediaType,
                resp => new OpenApiMediaType
                {
                    Schema = (apiResponseType.Type != null && apiResponseType.Type != typeof(void))
                        ? schemaRegistry.GetOrRegister(apiResponseType.Type)
                        : null
                }
            );

            return new OpenApiResponse
            {
                Description = description,
                Content = content
            };
        }

        private static readonly Dictionary<string, string> ResponseDescriptionMap = new Dictionary<string, string>
        {
            { "1\\d{2}", "Information" },
            { "2\\d{2}", "Success" },
            { "3\\d{2}", "Redirect" },
            { "400", "Bad Request" },
            { "401", "Unauthorized" },
            { "403", "Forbidden" },
            { "404", "Not Found" },
            { "405", "Method Not Allowed" },
            { "406", "Not Acceptable" },
            { "408", "Request Timeout" },
            { "409", "Conflict" },
            { "4\\d{2}", "Client Error" },
            { "5\\d{2}", "Server Error" }
        };

        private bool IsParameter(ApiParameterDescription parameterDescription)
        {
            return parameterDescription.Source == BindingSource.Path
                || parameterDescription.Source == BindingSource.Query
                || parameterDescription.Source == BindingSource.ModelBinding
                || parameterDescription.Source == BindingSource.Header;
        }

        private bool IsRequestBody(ApiParameterDescription parameterDescription)
        {
            return parameterDescription.Source == BindingSource.Body
                   || parameterDescription.Source == BindingSource.Form
                   || parameterDescription.Source == BindingSource.FormFile;
        }
    }
}
