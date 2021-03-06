﻿using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenSwagger.AspNetCore.ApiExplorer;
using OpenSwagger.Core.Model;

namespace OpenSwagger.AspNetCore.Application
{
    public class SwaggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISwaggerProvider _swaggerProvider;
        private readonly JsonSerializer _swaggerSerializer;
        private readonly SwaggerOptions _options;
        private readonly TemplateMatcher _requestMatcher;

        public SwaggerMiddleware(
            RequestDelegate next,
            ISwaggerProvider swaggerProvider,
            IOptions<MvcJsonOptions> mvcJsonOptions,
            SwaggerOptions options)
        {
            _next = next;
            _swaggerProvider = swaggerProvider;
            _swaggerSerializer = SwaggerSerializerFactory.Create(mvcJsonOptions);
            _options = options;
            _requestMatcher = new TemplateMatcher(TemplateParser.Parse(options.RouteTemplate), new RouteValueDictionary());
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string documentName;
            if (!RequestingSwaggerDocument(httpContext.Request, out documentName))
            {
                await _next(httpContext);
                return;
            }

            var basePath = string.IsNullOrEmpty(httpContext.Request.PathBase)
                ? "/"
                : httpContext.Request.PathBase.ToString();

            var host = httpContext.Request.Host.ToUriComponent();

            var swagger = _swaggerProvider.GetSwagger(documentName, host, basePath);

            // One last opportunity to modify the Swagger Document - this time with request context
            foreach (var filter in _options.PreSerializeFilters)
            {
                filter(swagger, httpContext.Request);
            }

            RespondWithSwaggerJson(httpContext.Response, swagger);
        }

        private bool RequestingSwaggerDocument(HttpRequest request, out string documentName)
        {
            documentName = null;
            if (request.Method != "GET") return false;

			var routeValues = new RouteValueDictionary();
            if (!_requestMatcher.TryMatch(request.Path, routeValues) || !routeValues.ContainsKey("documentName")) return false;

            documentName = routeValues["documentName"].ToString();
            return true;
        }

        private void RespondWithSwaggerJson(HttpResponse response, OpenApiDocument swagger)
        {
            response.StatusCode = 200;
            response.ContentType = "application/json";

            using (var writer = new StreamWriter(response.Body, System.Text.Encoding.UTF8, 1024, true))
            {
                _swaggerSerializer.Serialize(writer, swagger);
            }
        }
    }
}
