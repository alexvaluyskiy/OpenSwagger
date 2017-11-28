using System.Collections.Generic;
using OpenSwagger.Core.Model;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.TestFixtures.Extensions
{
    public class RecursiveCallSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema model, SchemaFilterContext context)
        {
            model.Properties = new Dictionary<string, Schema>();
            model.Properties.Add("ExtraProperty", context.SchemaRegistry.GetOrRegister(typeof(ComplexType)));
        }
    }
}
