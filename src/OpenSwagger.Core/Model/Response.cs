using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class Response
    {
        public string Description { get; set; }

        public IDictionary<string, Header> Headers { get; set; }

        public IDictionary<string, MediaType> Content { get; set; }

        public IDictionary<string, Link> Links { get; set; }
    }
}
