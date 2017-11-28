using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class RequestBody
    {
        public string Description { get; set; }

        public IDictionary<string, MediaType> Content { get; set; }

        public bool Required { get; set; }
    }
}
