using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class Operation
    {
        public IEnumerable<string> Tags { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public ExternalDocs ExternalDocs { get; set; }

        public string OperationId { get; set; }

        public IReadOnlyList<Parameter> Parameters { get; set; }

        public RequestBody RequestBody { get; set; }

        public IDictionary<string, Response> Responses { get; set; }

        // TODO: wrong type
        public IDictionary<string, object> Callbacks { get; set; }

        public bool? Deprecated { get; set; }

        public IDictionary<string, IEnumerable<string>> Security { get; set; }

        public IEnumerable<Server> Servers { get; set; }
    }
}
