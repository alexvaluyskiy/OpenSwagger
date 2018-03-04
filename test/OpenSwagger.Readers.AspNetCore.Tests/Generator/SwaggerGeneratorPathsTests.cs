using FluentAssertions;
using Microsoft.OpenApi.Models;
using OpenSwagger.AspNetCore.ApiExplorer.Tests;
using Xunit;

namespace OpenSwagger.Readers.AspNetCore.Tests.Generator
{
    public class SwaggerGeneratorPathsTests
    {
        [Fact]
        public void GenerateDocument_GenerateOperation_Get()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.AcceptsNothing)));

            swagger.Paths["/collection"].Operations[OperationType.Get].Should().NotBeNull();
        }

        [Fact]
        public void GenerateDocument_GenerateOperation_Post()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("POST", "collection", nameof(FakeActions.AcceptsNothing)));

            swagger.Paths["/collection"].Operations[OperationType.Post].Should().NotBeNull();
        }

        [Fact]
        public void GenerateDocument_GenerateOperation_Put()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("PUT", "collection", nameof(FakeActions.AcceptsNothing)));

            swagger.Paths["/collection"].Operations[OperationType.Put].Should().NotBeNull();
        }

        [Fact]
        public void GenerateDocument_GenerateOperation_Patch()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("PATCH", "collection", nameof(FakeActions.AcceptsNothing)));

            swagger.Paths["/collection"].Operations[OperationType.Patch].Should().NotBeNull();
        }

        [Fact]
        public void GenerateDocument_GenerateOperation_Delete()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("DELETE", "collection", nameof(FakeActions.AcceptsNothing)));

            swagger.Paths["/collection"].Operations[OperationType.Delete].Should().NotBeNull();
        }

        [Fact]
        public void GenerateDocument_GenerateOperation_Options()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("OPTIONS", "collection", nameof(FakeActions.AcceptsNothing)));

            swagger.Paths["/collection"].Operations[OperationType.Options].Should().NotBeNull();
        }

        [Fact]
        public void GenerateDocument_GenerateOperation_Head()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("HEAD", "collection", nameof(FakeActions.AcceptsNothing)));

            swagger.Paths["/collection"].Operations[OperationType.Head].Should().NotBeNull();
        }

        [Fact]
        public void GenerateDocument_GenerateOperation_Trace()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("TRACE", "collection", nameof(FakeActions.AcceptsNothing)));

            swagger.Paths["/collection"].Operations[OperationType.Trace].Should().NotBeNull();
        }
    }
}