using System;
using System.Collections.Generic;
using OpenSwagger.AspNetCore.ApiExplorer;
using OpenSwagger.Core.Model;

namespace Basic.Swagger
{
    public class OperationServersOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Servers = new List<Server>
            {
                new Server
                {
                    Url = "http://operation-level-server.swagger.io/v1",
                    Description = "Operation Level Server"
                }
            };
        }
    }
}
