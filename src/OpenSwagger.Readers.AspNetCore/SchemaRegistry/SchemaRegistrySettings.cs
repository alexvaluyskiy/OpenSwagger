using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace OpenSwagger.Readers.AspNetCore.Generator
{
    public class SchemaRegistrySettings
    {
        public SchemaRegistrySettings()
        {
            CustomTypeMappings = new Dictionary<Type, Func<OpenApiSchema>>();
            SchemaIdSelector = (type) => type.FriendlyId(false);
            SchemaFilters = new List<ISchemaFilter>();
        }

        public IDictionary<Type, Func<OpenApiSchema>> CustomTypeMappings { get; private set; }

        public bool DescribeAllEnumsAsStrings { get; set; }

        public bool DescribeStringEnumsInCamelCase { get; set; }

        public Func<Type, string> SchemaIdSelector { get; set; }

        public bool IgnoreObsoleteProperties { get; set; }

        public IList<ISchemaFilter> SchemaFilters { get; private set; }

        public SchemaRegistrySettings Clone()
        {
            return new SchemaRegistrySettings
            {
                CustomTypeMappings = CustomTypeMappings,
                DescribeAllEnumsAsStrings = DescribeAllEnumsAsStrings,
                DescribeStringEnumsInCamelCase = DescribeStringEnumsInCamelCase,
                IgnoreObsoleteProperties = IgnoreObsoleteProperties,
                SchemaIdSelector = SchemaIdSelector,
                SchemaFilters = SchemaFilters
            };
        }
    }
}