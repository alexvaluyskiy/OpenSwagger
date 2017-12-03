using System.Collections.Generic;
using OpenSwagger.AspNetCore.ApiExplorer;
using OpenSwagger.Core.Model;

namespace Basic.Swagger
{
    public class ResponseHeadersOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Responses["200"].Headers = new Dictionary<string, Header>
            {
                ["X-Rate-Limit"] = new Header
                {
                    Description = "The number of allowed requests in the current period",
                    Schema = new Schema
                    {
                        Type = "integer"
                    }
                }
            };
        }
    }
}
