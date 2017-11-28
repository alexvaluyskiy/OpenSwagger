using Newtonsoft.Json;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests
{
    public class JsonAnnotatedType
    {
        [JsonIgnore]
        public int Property1 { get; set; }

        [JsonProperty("foobar")]
        public string Property2 { get; set; }

        [JsonProperty(Required = Required.Always)]
        public decimal Property3 { get; set; }
    }
}