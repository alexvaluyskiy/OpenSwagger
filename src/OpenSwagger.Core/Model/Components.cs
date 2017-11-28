using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class Components
    {
        public IDictionary<string, Schema> Schemas { get; set; }

        public IDictionary<string, Response> Responses { get; set; }

        public IDictionary<string, Parameter> Parameters { get; set; }

        public IDictionary<string, Example> Examples { get; set; }

        public IDictionary<string, RequestBody> RequestBodies { get; set; }

        public IDictionary<string, Header> Headers { get; set; }

        public IDictionary<string, SecurityScheme> SecuritySchemes { get; set; }

        public IDictionary<string, Link> Links { get; set; }

        // TODO: wrong type
        public IDictionary<string, object> Callbacks { get; set; }
    }
}
