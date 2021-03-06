﻿using Newtonsoft.Json;

namespace OpenSwagger.AspNetCore.ApiExplorer
{
    public class SchemaRegistryFactory : ISchemaRegistryFactory
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly SchemaRegistrySettings _schemaRegistrySettings;

        public SchemaRegistryFactory(
            JsonSerializerSettings jsonSerializerSettings,
            SchemaRegistrySettings schemaRegistrySettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings;
            _schemaRegistrySettings = schemaRegistrySettings;
        }

        public ISchemaRegistry Create()
        {
            return new SchemaRegistry(_jsonSerializerSettings, _schemaRegistrySettings);
        }
    }
}
