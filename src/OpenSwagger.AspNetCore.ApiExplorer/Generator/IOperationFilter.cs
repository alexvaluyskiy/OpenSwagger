using Microsoft.AspNetCore.Mvc.ApiExplorer;
using OpenSwagger.Core.Model;

namespace OpenSwagger.AspNetCore.ApiExplorer
{
    public interface IOperationFilter
    {
        void Apply(Operation operation, OperationFilterContext context);
    }

    public class OperationFilterContext
    {
        public OperationFilterContext(ApiDescription apiDescription, ISchemaRegistry schemaRegistry)
        {
            ApiDescription = apiDescription;
            SchemaRegistry = schemaRegistry;
        }

        public ApiDescription ApiDescription { get; }

        public ISchemaRegistry SchemaRegistry { get; }
    }
}
