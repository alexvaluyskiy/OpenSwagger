using System;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests
{
    public class ObsoletePropertiesType
    {
        public string Property1 { get; set; }

        [Obsolete]
        public string ObsoleteProperty { get; set; }
    }
}