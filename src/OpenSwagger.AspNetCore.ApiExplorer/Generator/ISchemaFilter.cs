using System;
using Newtonsoft.Json.Serialization;
using OpenSwagger.Core.Model;

namespace OpenSwagger.AspNetCore.ApiExplorer
{
    public interface ISchemaFilter
    {
        void Apply(Schema model, SchemaFilterContext context);
    }

    public class SchemaFilterContext
    {
        public SchemaFilterContext(
            Type systemType,
            JsonContract jsonContract,
            ISchemaRegistry schemaRegistry)
        {
            SystemType = systemType;
            JsonContract = jsonContract;
            SchemaRegistry = schemaRegistry;
        }

        public Type SystemType { get; }

        public JsonContract JsonContract { get; }

        public ISchemaRegistry SchemaRegistry { get; }
    }
}