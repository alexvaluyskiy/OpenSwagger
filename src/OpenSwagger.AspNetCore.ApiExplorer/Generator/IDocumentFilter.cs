using Microsoft.AspNetCore.Mvc.ApiExplorer;
using OpenSwagger.Core.Model;

namespace OpenSwagger.AspNetCore.ApiExplorer
{
    public interface IDocumentFilter
    {
        void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context);
    }

    public class DocumentFilterContext
    {
        public DocumentFilterContext(
            ApiDescriptionGroupCollection apiDescriptionsGroups,
            ISchemaRegistry schemaRegistry)
        {
            ApiDescriptionsGroups = apiDescriptionsGroups;
            SchemaRegistry = schemaRegistry;
        }

        public ApiDescriptionGroupCollection ApiDescriptionsGroups { get; }

        public ISchemaRegistry SchemaRegistry { get; }
    }
}
