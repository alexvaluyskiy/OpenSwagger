using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class Encoding
    {
        public string ContentType { get; set; }

        public IDictionary<string, Header> Headers { get; set; }

        public string Style { get; set; }

        public bool Explode { get; set; }

        public bool AllowReserved { get; set; }
    }
}
