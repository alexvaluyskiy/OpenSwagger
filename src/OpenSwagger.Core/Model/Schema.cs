using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenSwagger.Core.Model
{
    public sealed class Schema
    {
        [JsonProperty("$ref")]
        public string Ref { get; set; }

        public string Title { get; set; }

        public int? MultipleOf { get; set; }

        public int? Maximum { get; set; }

        public bool? ExclusiveMaximum { get; set; }

        public int? Minimum { get; set; }

        public bool? ExclusiveMinimum { get; set; }

        public int? MaxLength { get; set; }

        public int? MinLength { get; set; }

        public string Pattern { get; set; }

        public int? MaxItems { get; set; }

        public int? MinItems { get; set; }

        public bool? UniqueItems { get; set; }

        public int? MaxProperties { get; set; }

        public int? MinProperties { get; set; }

        public IEnumerable<string> Required { get; set; }

        public IEnumerable<object> Enum { get; set; }

        public string Type { get; set; }

        public IEnumerable<Schema> AllOf { get; set; }

        public IEnumerable<Schema> OneOf { get; set; }

        public IEnumerable<Schema> AnyOf { get; set; }

        public IEnumerable<Schema> Not { get; set; }

        public Schema Items { get; set; }

        public IDictionary<string, Schema> Properties { get; set; }

        public Schema AdditionalProperties { get; set; }

        public string Description { get; set; }

        public string Format { get; set; }

        public object Default { get; set; }

        public bool? Nullable { get; set; }

        public string Discriminator { get; set; }

        public bool? ReadOnly { get; set; }

        public bool? WriteOnly { get; set; }

        public object Xml { get; set; }

        public ExternalDocs ExternalDocs { get; set; }

        public object Example { get; set; }

        public bool? Deprecated { get; set; }
    }
}
