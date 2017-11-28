using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OpenSwagger.Core.Model
{
    public class Parameter
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter), true)]
        public ParameterLocation In { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        public bool? Deprecated { get; set; }

        public bool? AllowEmptyValue { get; set; }

        public string Style { get; set; }

        public bool? Explode { get; set; }

        public bool? AllowReserved { get; set; }

        public Schema Schema { get; set; }

        public object Example { get; set; }

        public IDictionary<string, Example> Examples { get; set; }

        public IDictionary<string, MediaType> Content { get; set; }
    }

    public enum ParameterLocation
    {
        Path,
        Query,
        Header,
        Cookie
    }
}
