using System;
using System.Collections.Generic;
using OpenSwagger.AspNetCore.ApiExplorer;
using OpenSwagger.Core.Model;

namespace Basic.Swagger
{
    public sealed class GlobalSecurityDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext context)
        {
            openApiDocument.Security.Add(new Dictionary<string, IEnumerable<string>>
            {
                ["OAuth2"] = new List<string> { "read" }
            });
        }
    }
}
