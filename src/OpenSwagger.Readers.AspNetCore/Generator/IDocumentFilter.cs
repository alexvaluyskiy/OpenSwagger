using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace OpenSwagger.Readers.AspNetCore
{
    public interface IDocumentFilter
    {
        void Apply(OpenApiDocument swaggerDocument, DocumentFilterContext context);
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

        public ApiDescriptionGroupCollection ApiDescriptionsGroups { get; private set; }

        public ISchemaRegistry SchemaRegistry { get; private set; }
    }
}