using System.Collections.Generic;
using Basic.Controllers;
using OpenSwagger.AspNetCore.ApiExplorer;
using OpenSwagger.Core.Model;

namespace Basic.Swagger
{
    public class ResponseExamplesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var content = operation.Responses["200"].Content;
            foreach (var mediaType in content.Keys)
            {
                content[mediaType].Examples = new Dictionary<string, Example>
                {
                    ["foo"] = new Example
                    {
                        Summary = "A foo example",
                        Value = new Cart { Id = 5 }
                    }
                };
            }
        }
    }
}
