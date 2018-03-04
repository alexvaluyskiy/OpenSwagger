using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests
{
    public class ExtensionDataObjectType
    {
        public bool Property1 { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object> ExtensionData { get; set; }
    }
}