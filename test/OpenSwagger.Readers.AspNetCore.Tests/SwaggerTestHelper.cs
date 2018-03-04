using System;
using Newtonsoft.Json;
using OpenSwagger.AspNetCore.ApiExplorer;
using OpenSwagger.AspNetCore.ApiExplorer.Tests;
using OpenSwagger.Core.Model;

namespace OpenSwagger.Readers.AspNetCore.Tests
{
    public static class SwaggerTestHelper
    {
        public static Microsoft.OpenApi.Models.OpenApiDocument GenerateDocument(
            Action<FakeApiDescriptionGroupCollectionProvider> setupApis = null)
        {
            var apiDescriptionsProvider = new FakeApiDescriptionGroupCollectionProvider();
            setupApis?.Invoke(apiDescriptionsProvider);

            var settings = new ApiExplorerReaderSettings();
            var reader = new ApiExplorerReader(settings);
            var document = reader.Read(apiDescriptionsProvider, out var diagnostic);

            return document;
        }
    }
}
