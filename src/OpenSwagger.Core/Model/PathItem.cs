using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenSwagger.Core.Model
{
    public sealed class PathItem
    {
        [JsonProperty("$ref")]
        public string Ref { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public Operation Get { get; set; }

        public Operation Put { get; set; }

        public Operation Post { get; set; }

        public Operation Delete { get; set; }

        public Operation Options { get; set; }

        public Operation Head { get; set; }

        public Operation Patch { get; set; }

        public Operation Trace { get; set; }

        public IEnumerable<Server> Servers { get; set; }

        public IEnumerable<Parameter> Parameters { get; set; }
    }
}
