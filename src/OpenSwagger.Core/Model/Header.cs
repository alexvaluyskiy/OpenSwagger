using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class Header
    {
        public string Description { get; set; }

        public bool? Required { get; set; }

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
}
