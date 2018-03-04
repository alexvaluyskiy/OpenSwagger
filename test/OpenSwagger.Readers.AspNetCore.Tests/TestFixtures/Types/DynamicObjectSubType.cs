using System.Dynamic;
using Newtonsoft.Json;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests
{
    [JsonObject]
    public class DynamicObjectSubType : DynamicObject
    {
        public string Property1 { get; set; }
    }
}