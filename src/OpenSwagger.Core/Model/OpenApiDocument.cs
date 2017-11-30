using System.Collections;
using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class OpenApiDocument
    {
        public string OpenApi { get; } = "3.0.0";

        public Info Info { get; set; }

        public IEnumerable<Server> Servers { get; set; }

        public IDictionary<string, PathItem> Paths { get; set; }

        public Components Components { get; set; }

        public IDictionary<string, IEnumerable<string>> Security { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public IEnumerable<ExternalDocs> ExternalDocs { get; set; }
    }
}
