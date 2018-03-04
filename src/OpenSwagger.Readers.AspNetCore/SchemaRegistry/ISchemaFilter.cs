using System;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace OpenSwagger.Readers.AspNetCore.Generator
{
    public interface ISchemaFilter
    {
        void Apply(OpenApiSchema model, SchemaFilterContext context);
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