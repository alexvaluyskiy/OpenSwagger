using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class MediaType
    {
        public Schema Schema { get; set; }

        public object Example { get; set; }

        public IDictionary<string, Example> Examples { get; set; }

        public IDictionary<string, Encoding> Encoding { get; set; }
    }
}
