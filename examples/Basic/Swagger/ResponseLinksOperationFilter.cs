using System;
using System.Collections.Generic;
using OpenSwagger.AspNetCore.ApiExplorer;
using OpenSwagger.Core.Model;

namespace Basic.Swagger
{
    public class ResponseLinksOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Responses["200"].Links = new Dictionary<string, Link>
            {
                ["address"] = new Link
                {
                    OperationId = "getUserAddress",
                    Parameters = new Dictionary<string, object>
                    {
                        ["userId"] = "$request.path.id"
                    }
                }
            };
        }
    }
}
