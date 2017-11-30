using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using OpenSwagger.Core.Model;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests
{
    public static class SwaggerTestHelper
    {
        public static SwaggerGenerator Subject(
            Action<FakeApiDescriptionGroupCollectionProvider> setupApis = null,
            Action<SwaggerGeneratorSettings> configure = null)
        {
            var apiDescriptionsProvider = new FakeApiDescriptionGroupCollectionProvider();
            setupApis?.Invoke(apiDescriptionsProvider);

            var options = new SwaggerGeneratorSettings();
            options.SwaggerDocs.Add("v1", new Info { Title = "API", Version = "v1" });

            configure?.Invoke(options);

            return new SwaggerGenerator(
                apiDescriptionsProvider,
                new SchemaRegistryFactory(new JsonSerializerSettings(), new SchemaRegistrySettings()),
                options
            );
        }
    }
}
