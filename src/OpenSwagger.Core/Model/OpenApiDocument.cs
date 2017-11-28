using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public class OpenApiDocument
    {
        public string OpenApi { get; } = "3.0.0";

        public Info Info { get; set; }

        public IEnumerable<Server> Servers { get; set; }

        public IDictionary<string, PathItem> Paths { get; set; }

        public Components Components { get; set; }

        // TODO: wrong type
        public object Security { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public IEnumerable<ExternalDocs> ExternalDocs { get; set; }
    }
}
