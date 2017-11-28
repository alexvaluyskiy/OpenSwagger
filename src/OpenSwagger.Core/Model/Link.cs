using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class Link
    {
        public string OperationRef { get; set; }

        public string OperationId { get; set; }

        public IDictionary<string, object> Parameters { get; set; }

        public object RequestBody { get; set; }

        public string Description { get; set; }

        public Server Server { get; set; }
    }
}
