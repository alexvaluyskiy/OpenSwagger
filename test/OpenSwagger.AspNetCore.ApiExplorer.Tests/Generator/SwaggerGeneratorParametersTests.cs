using System;
using System.Linq;
using Newtonsoft.Json;
using OpenSwagger.Core.Model;
using Xunit;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.Generator
{
    public class SwaggerGeneratorParametersTests
    {
        [Fact]
        public void GetSwagger_SetsParametersToNull_ForParameterlessActions()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection"].Get;
            Assert.Null(operation.Parameters);
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
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis.Add("GET", routeTemplate, actionFixtureName));

            var swagger = subject.GetSwagger("v1");

            var param = swagger.Paths["/" + routeTemplate].Get.Parameters.First();
            Assert.IsAssignableFrom<Parameter>(param);
            Assert.NotNull(param);
            Assert.Equal("param", param.Name);
            Assert.Equal(expectedIn, param.In);
            Assert.Null(swagger.Paths["/" + routeTemplate].Get.RequestBody);
        }

        [Fact]
        public void GetSwagger_SetsStyleFormAndExplodeTrue_ForQueryBoundArrayParams()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", "resource", nameof(FakeActions.AcceptsArrayFromQuery)));

            var swagger = subject.GetSwagger("v1");

            var param = swagger.Paths["/resource"].Get.Parameters.First();
            Assert.Equal("form", param.Style);
            Assert.Equal(true, param.Explode);
        }

        [Fact]
        public void GetSwagger_GeneratesQueryParams_ForAllUnboundParams()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.AcceptsUnboundStringParameter))
                .Add("POST", "collection", nameof(FakeActions.AcceptsUnboundComplexParameter)));

            var swagger = subject.GetSwagger("v1");

            var getParam = swagger.Paths["/collection"].Get.Parameters.First();
            Assert.Equal(ParameterLocation.Query, getParam.In);
            // Multiple post parameters as ApiExplorer flattens out the complex type
            var postParams = swagger.Paths["/collection"].Post.Parameters;
            Assert.All(postParams, (p) => Assert.Equal(ParameterLocation.Query, p.In));
        }

        [Theory]
        [InlineData("collection/{param}")]
        [InlineData("collection/{param?}")]
        public void GetSwagger_SetsParameterRequired_ForAllRouteParams(string routeTemplate)
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", routeTemplate, nameof(FakeActions.AcceptsStringFromRoute)));

            var swagger = subject.GetSwagger("v1");

            var param = swagger.Paths["/collection/{param}"].Get.Parameters.First();
            Assert.Equal(true, param.Required);
        }

        [Theory]
        [InlineData(nameof(FakeActions.AcceptsStringFromQuery), false)]
        [InlineData(nameof(FakeActions.AcceptsIntegerFromQuery), true)]
        public void GetSwagger_SetsParameterRequired_ForNonNullableActionParams(
            string actionFixtureName, bool expectedRequired)
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis.Add("GET", "collection", actionFixtureName));

            var swagger = subject.GetSwagger("v1");

            var param = swagger.Paths["/collection"].Get.Parameters.First();
            Assert.Equal(expectedRequired, param.Required);
        }

        [Theory]
        [InlineData("Property2", false)] // DateTime
        [InlineData("Property1", true)] // bool
        [InlineData("Property4", true)] // string with RequiredAttribute
        public void GetSwagger_SetsParameterRequired_ForNonNullableOrExplicitlyRequiredPropertyBasedParams(
            string paramName, bool expectedRequired)
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.AcceptsComplexTypeFromQuery)));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection"].Get;
            var param = operation.Parameters.First(p => p.Name == paramName);
            Assert.Equal(expectedRequired, param.Required);
        }

        [Fact(Skip = "Not implemented yet")]
        public void GetSwagger_SetsParameterTypeString_ForUnboundRouteParams()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", "collection/{param}", nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            var param = swagger.Paths["/collection/{param}"].Get.Parameters.First();
            Assert.IsAssignableFrom<Parameter>(param);
            Assert.Equal("param", param.Name);
            Assert.Equal(ParameterLocation.Path, param.In);
            Assert.Equal("string", param.Schema.Type);
        }

        [Fact]
        public void GetSwagger_IgnoresParameters_IfPartOfCancellationToken()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.AcceptsCancellationToken)));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection"].Get;
            Assert.Null(operation.Parameters);
        }

        [Fact]
        public void GetSwagger_DescribesParametersInCamelCase_IfSpecifiedBySettings()
        {
            var subject = SwaggerTestHelper.Subject(
                setupApis: apis => apis.Add("GET", "collection", nameof(FakeActions.AcceptsComplexTypeFromQuery)),
                configure: c => c.DescribeAllParametersInCamelCase = true
            );

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection"].Get;
            Assert.Equal(5, operation.Parameters.Count);
            Assert.Equal("property1", operation.Parameters[0].Name);
            Assert.Equal("property2", operation.Parameters[1].Name);
            Assert.Equal("property3", operation.Parameters[2].Name);
            Assert.Equal("property4", operation.Parameters[3].Name);
            Assert.Equal("property5", operation.Parameters[4].Name);
        }
    }
}