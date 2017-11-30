using System;
using FluentAssertions;
using Xunit;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.Generator
{
    public class SwaggerGeneratorRequestBodiesTests
    {
        [Fact(Skip = "Not implemented yet")]
        public void GetSwagger_GeneratesRequestBody_PostBodyUnboundParameters()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("POST", "collection", nameof(FakeActions.AcceptsUnboundComplexParameter)));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection"].Post;
            operation.Should().NotBeNull();
            operation.RequestBody.Should().NotBeNull();
            operation.RequestBody.Content.Should().ContainKey("application/json");
            var content = operation.RequestBody.Content["application/json"];
            content.Schema.Should().NotBeNull();
            content.Schema.Ref.Should().Be("#/components/schemas/ComplexType");
            swagger.Components.Schemas.Should().ContainKey("ComplexType");
        }

        [Fact(Skip = "Not implemented yet")]
        public void GetSwagger_GeneratesRequestBody_PutBodyUnboundParameters()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("PUT", "collection/{param}", nameof(FakeActions.AcceptsUnboundComplexParameter)));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection/{param}"].Put;
            operation.Should().NotBeNull();
            operation.RequestBody.Should().NotBeNull();
            operation.RequestBody.Content.Should().ContainKey("application/json");
            var content = operation.RequestBody.Content["application/json"];
            content.Schema.Should().NotBeNull();
            content.Schema.Ref.Should().Be("#/components/schemas/ComplexType");
            swagger.Components.Schemas.Should().ContainKey("ComplexType");
        }

        [Fact]
        public void GetSwagger_GeneratesRequestBody_PostBodyBoundParameters()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("POST", "collection", nameof(FakeActions.AcceptsComplexTypeFromBody)));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection"].Post;
            operation.Should().NotBeNull();
            operation.RequestBody.Should().NotBeNull();
            operation.RequestBody.Content.Should().ContainKey("application/json");
            var content = operation.RequestBody.Content["application/json"];
            content.Schema.Should().NotBeNull();
            content.Schema.Ref.Should().Be("#/components/schemas/ComplexType");
            swagger.Components.Schemas.Should().ContainKey("ComplexType");
        }

        [Fact]
        public void GetSwagger_GeneratesRequestBody_PutBodyBoundParameters()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("PUT", "collection/{param}", nameof(FakeActions.AcceptsComplexTypeFromBody)));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection/{param}"].Put;
            operation.Should().NotBeNull();
            operation.RequestBody.Should().NotBeNull();
            operation.RequestBody.Content.Should().ContainKey("application/json");
            var content = operation.RequestBody.Content["application/json"];
            content.Schema.Should().NotBeNull();
            content.Schema.Ref.Should().Be("#/components/schemas/ComplexType");
            swagger.Components.Schemas.Should().ContainKey("ComplexType");
        }

        [Fact(Skip = "Not implemented yet")]
        public void GetSwagger_GeneratesRequestBody_ForFormFile()
        {
            var routeTemplate = "collection";
            var actionFixtureName = nameof(FakeActions.AcceptsFormFileType);
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("POST", routeTemplate, actionFixtureName));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/" + routeTemplate].Post;
            operation.Should().NotBeNull();
            operation.RequestBody.Should().NotBeNull();
            operation.RequestBody.Content.Should().ContainKey("multipart/form-data");

            var content = operation.RequestBody.Content["multipart/form-data"];
            content.Schema.Should().NotBeNull();
            content.Schema.Properties.Should().ContainKey("file");
            var property = content.Schema.Properties["file"];
            property.Type.Should().Be("string");
            property.Format.Should().Be("binary");
        }

        [Fact(Skip = "Not implemented yet")]
        public void GetSwagger_GeneratesRequestBody_ForMultipleFormFile()
        {
            var routeTemplate = "collection";
            var actionFixtureName = nameof(FakeActions.AcceptsFormFileListType);
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("POST", routeTemplate, actionFixtureName));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/" + routeTemplate].Post;
            operation.Should().NotBeNull();
            operation.RequestBody.Should().NotBeNull();
            operation.RequestBody.Content.Should().ContainKey("multipart/form-data");

            var content = operation.RequestBody.Content["multipart/form-data"];
            content.Schema.Should().NotBeNull();

            content.Schema.Properties.Should().ContainKey("file");
            var property = content.Schema.Properties["file"];
            property.Type.Should().Be("array");
            property.Items.Type.Should().Be("string");
            property.Items.Format.Should().Be("binary");
        }

        [Fact(Skip = "Not supported by ASP.NET Core")]
        public void GetSwagger_GeneratesRequestBody_WithCustomContentType()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("POST", "collection", nameof(FakeActions.AcceptsComplexTypeWithConsumes)));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection"].Post;
            operation.Should().NotBeNull();
            operation.RequestBody.Should().NotBeNull();
            operation.RequestBody.Content.Should().HaveCount(1);
            operation.RequestBody.Content.Should().ContainKey("application/custom");
        }

        [Fact(Skip = "Not supported")]
        public void GetSwagger_GeneratesRequestBodyForm_PostBodyBoundParameters()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("POST", "collection", nameof(FakeActions.AcceptsStringFromForm)));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection"].Post;
            operation.Should().NotBeNull();
            operation.RequestBody.Should().NotBeNull();
            operation.RequestBody.Content.Should().ContainKey("multipart/form-data");
            // TODO
        }
    }
}