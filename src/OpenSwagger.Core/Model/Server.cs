using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class Server
    {
        public string Url { get; set; }

        public string Description { get; set; }

        public IDictionary<string, ServerVariable> Variables { get; set; }
    }

    public sealed class ServerVariable
    {
        public IEnumerable<string> Enum { get; set; } = new List<string>();

        public string Default { get; set; }

        public string Description { get; set; }
    }
}
