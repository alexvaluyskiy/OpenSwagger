using System;
using System.Collections.Generic;
using OpenSwagger.AspNetCore.ApiExplorer;
using OpenSwagger.Core.Model;

namespace Basic.Swagger
{
    public class ResponseFilesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Responses["200"].Content = new Dictionary<string, MediaType>
            {
                ["application/octet-stream"] = new MediaType
                {
                    Schema = new Schema
                    {
                        Type = "string",
                        Format = "binary"
                    }
                }
            };
        }
    }
}
