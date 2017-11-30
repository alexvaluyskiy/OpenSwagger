using FluentAssertions;
using Xunit;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.Generator
{
    public class SwaggerGeneratorPathsTests
    {
        [Fact]
        public void GetSwagger_GenerateOperation_Get()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            swagger.Paths["/collection"].Get.Should().NotBeNull();
        }

        [Fact]
        public void GetSwagger_GenerateOperation_Post()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("POST", "collection", nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            swagger.Paths["/collection"].Post.Should().NotBeNull();
        }

        [Fact]
        public void GetSwagger_GenerateOperation_Put()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("PUT", "collection", nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            swagger.Paths["/collection"].Put.Should().NotBeNull();
        }

        [Fact]
        public void GetSwagger_GenerateOperation_Patch()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("PATCH", "collection", nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            swagger.Paths["/collection"].Patch.Should().NotBeNull();
        }

        [Fact]
        public void GetSwagger_GenerateOperation_Delete()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("DELETE", "collection", nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            swagger.Paths["/collection"].Delete.Should().NotBeNull();
        }

        [Fact]
        public void GetSwagger_GenerateOperation_Options()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("OPTIONS", "collection", nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            swagger.Paths["/collection"].Options.Should().NotBeNull();
        }

        [Fact]
        public void GetSwagger_GenerateOperation_Head()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("HEAD", "collection", nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            swagger.Paths["/collection"].Head.Should().NotBeNull();
        }

        [Fact]
        public void GetSwagger_GenerateOperation_Trace()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("TRACE", "collection", nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            swagger.Paths["/collection"].Trace.Should().NotBeNull();
        }
    }
}