using System.Collections.Generic;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests
{
    public class CompositeType
    {
        public ComplexType Property1 { get; set; }

        public IEnumerable<ComplexType> Property2 { get; set; }
    }
}