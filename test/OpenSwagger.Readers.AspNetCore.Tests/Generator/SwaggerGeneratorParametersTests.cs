using System.Linq;
using FluentAssertions;
using Microsoft.OpenApi.Models;
using OpenSwagger.AspNetCore.ApiExplorer.Tests;
using Xunit;

namespace OpenSwagger.Readers.AspNetCore.Tests.Generator
{
    public class SwaggerGeneratorParametersTests
    {
        [Fact]
        public void GetSwagger_SetsParametersToNull_ForParameterlessActions()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.AcceptsNothing)));

            swagger.Paths["/collection"].Operations[OperationType.Get].Parameters.Should().BeNull();
        }

        [Theory]
        [InlineData("collection/{param}", nameof(FakeActions.AcceptsStringFromRoute), ParameterLocation.Path)]
        [InlineData("collection", nameof(FakeActions.AcceptsStringFromQuery), ParameterLocation.Query)]
        [InlineData("collection", nameof(FakeActions.AcceptsStringFromHeader), ParameterLocation.Header)]
        public void GetSwagger_GeneratesParameters_ForPathQueryHeaderParams(
            string routeTemplate,
            string actionFixtureName,
            ParameterLocation expectedIn)
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis.Add("GET", routeTemplate, actionFixtureName));

            var parameters = swagger.Paths["/" + routeTemplate].Operations[OperationType.Get].Parameters;
            parameters.Should().HaveCount(1);
            var param = parameters.First();
            param.Name.Should().Be("param");
            param.In.Should().Be(expectedIn);
            swagger.Paths["/" + routeTemplate].Operations[OperationType.Get].RequestBody.Should().BeNull();
        }

        [Fact]
        public void GetSwagger_SetsStyleFormAndExplodeTrue_ForQueryBoundArrayParams()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("GET", "resource", nameof(FakeActions.AcceptsArrayFromQuery)));

            var param = swagger.Paths["/resource"].Operations[OperationType.Get].Parameters.First();
            param.Style.Should().NotBeNull();
            param.Style.Should().Be(ParameterStyle.Form);
            param.Explode.Should().BeTrue();
        }

        [Fact]
        public void GetSwagger_GeneratesQueryParams_ForAllUnboundParams()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.AcceptsUnboundStringParameter))
                .Add("POST", "collection", nameof(FakeActions.AcceptsUnboundComplexParameter)));

            var getParam = swagger.Paths["/collection"].Operations[OperationType.Get].Parameters.First();
            getParam.In.Should().Be(ParameterLocation.Query);

            // Multiple post parameters as ApiExplorer flattens out the complex type
            var postParams = swagger.Paths["/collection"].Operations[OperationType.Post].Parameters;
            postParams.Should().OnlyContain(c => c.In == ParameterLocation.Query);
        }

        [Theory]
        [InlineData("collection/{param}")]
        [InlineData("collection/{param?}")]
        public void GetSwagger_SetsParameterRequired_ForAllRouteParams(string routeTemplate)
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("GET", routeTemplate, nameof(FakeActions.AcceptsStringFromRoute)));

            var param = swagger.Paths["/collection/{param}"].Operations[OperationType.Get].Parameters.First();
            param.Required.Should().BeTrue();
        }

        [Theory]
        [InlineData(nameof(FakeActions.AcceptsStringFromQuery), false)]
        [InlineData(nameof(FakeActions.AcceptsIntegerFromQuery), true)]
        public void GetSwagger_SetsParameterRequired_ForNonNullableActionParams(
            string actionFixtureName, bool expectedRequired)
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis.Add("GET", "collection", actionFixtureName));

            var param = swagger.Paths["/collection"].Operations[OperationType.Get].Parameters.First();
            param.Required.Should().Be(expectedRequired);
        }

        [Theory]
        [InlineData("Property2", false)] // DateTime
        [InlineData("Property1", true)] // bool
        [InlineData("Property4", true)] // string with RequiredAttribute
        public void GetSwagger_SetsParameterRequired_ForNonNullableOrExplicitlyRequiredPropertyBasedParams(
            string paramName, bool expectedRequired)
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.AcceptsComplexTypeFromQuery)));

            var operation = swagger.Paths["/collection"].Operations[OperationType.Get];
            var param = operation.Parameters.First(p => p.Name == paramName);
            param.Required.Should().Be(expectedRequired);
        }

        [Fact]
        public void GetSwagger_SetsParameterTypeString_ForUnboundRouteParams()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("GET", "collection/{param}", nameof(FakeActions.AcceptsNothing)));

            var param = swagger.Paths["/collection/{param}"].Operations[OperationType.Get].Parameters.First();
            Assert.IsAssignableFrom<OpenApiParameter>(param);
            Assert.Equal("param", param.Name);
            //Assert.Equal(ParameterLocation.Path, param.In);
            Assert.Equal("string", param.Schema.Type);
        }

        [Fact]
        public void GetSwagger_IgnoresParameters_IfPartOfCancellationToken()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.AcceptsCancellationToken)));

            var operation = swagger.Paths["/collection"].Operations[OperationType.Get];
            operation.Parameters.Should().BeNull();
        }

        [Fact]
        public void GetSwagger_DescribesParametersInCamelCase_IfSpecifiedBySettings()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(
                setupApis: apis => apis.Add("GET", "collection", nameof(FakeActions.AcceptsComplexTypeFromQuery))
            );

            var operation = swagger.Paths["/collection"].Operations[OperationType.Get];
            operation.Parameters.Should().HaveCount(5);
            operation.Parameters[0].Name.Should().Be("property1");
            operation.Parameters[1].Name.Should().Be("property2");
            operation.Parameters[2].Name.Should().Be("property3");
            operation.Parameters[3].Name.Should().Be("property4");
            operation.Parameters[4].Name.Should().Be("property5");
        }
    }
}