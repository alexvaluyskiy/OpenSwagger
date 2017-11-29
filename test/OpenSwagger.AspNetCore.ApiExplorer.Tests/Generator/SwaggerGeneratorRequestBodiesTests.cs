using System;
using Newtonsoft.Json;
using OpenSwagger.Core.Model;
using Xunit;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.Generator
{
    public class SwaggerGeneratorRequestBodiesTests
    {
        [Fact]
        public void GetSwagger_GeneratesBodyParams_ForBodyBoundParams()
        {
            var subject = Subject(setupApis: apis => apis
                .Add("POST", "collection", nameof(FakeActions.AcceptsComplexTypeFromBody)));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection"].Post;
            Assert.NotNull(operation);
            Assert.NotNull(operation.RequestBody);
            Assert.True(operation.RequestBody.Content.ContainsKey("application/json"));
            var content = operation.RequestBody.Content["application/json"];
            Assert.NotNull(content.Schema);
            Assert.Equal("#/components/schemas/ComplexType", content.Schema.Ref);
            //Assert.Contains("ComplexType", swagger.Components.Definitions.Keys);
        }


        [Fact(Skip = "Not implemented yet")]
        public void GetSwagger_GeneratesParameters_ForFormFile()
        {
            var routeTemplate = "collection";
            var actionFixtureName = nameof(FakeActions.AcceptsFormFileType);
            var subject = Subject(setupApis: apis => apis.Add("POST", routeTemplate, actionFixtureName));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/" + routeTemplate].Post;
            Assert.NotNull(operation);
            Assert.NotNull(operation.RequestBody);
            Assert.True(operation.RequestBody.Content.ContainsKey("multipart/form-data"));
            var content = operation.RequestBody.Content["multipart/form-data"];
            Assert.NotNull(content.Schema);
            //Assert.Equal("#/components/schemas/ComplexType", content.Schema.Ref);
        }

        private SwaggerGenerator Subject(
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