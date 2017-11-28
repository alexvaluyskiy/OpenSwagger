using Microsoft.Extensions.Logging;
using Basic.Controllers;
using OpenSwagger.AspNetCore.ApiExplorer;
using OpenSwagger.Core.Model;

namespace Basic.Swagger
{
    public class AddCartDefault : ISchemaFilter
    {
        private ILogger<AddCartDefault> _logger;

        public AddCartDefault(ILogger<AddCartDefault> logger)
        {
            _logger = logger;
        }

        public void Apply(Schema schema, SchemaFilterContext context)
        {
            _logger.LogInformation("Applying an awesome Schema Filter that leverages Dependency Injection");

            schema.Default = new Cart
            {
                Id = 123,
            };
        }
    }
}