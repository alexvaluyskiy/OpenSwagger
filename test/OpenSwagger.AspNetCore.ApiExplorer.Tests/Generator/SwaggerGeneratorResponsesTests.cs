using System;
using System.Linq;
using Newtonsoft.Json;
using OpenSwagger.Core.Model;
using Xunit;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.Generator
{
    public class SwaggerGeneratorResponsesTests
    {
        [Theory]
        [InlineData(nameof(FakeActions.ReturnsVoid), "200", "Success")]
        [InlineData(nameof(FakeActions.ReturnsActionResult), "200", "Success")]
        public void GetSwagger_GeneratesResponsesFromReturnTypes_IfResponseTypeAttributesNotPresent_ForVoid(
            string actionFixtureName,
            string expectedStatusCode,
            string expectedDescriptions)
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis =>
                apis.Add("GET", "collection", actionFixtureName));

            var swagger = subject.GetSwagger("v1");

            var responses = swagger.Paths["/collection"].Get.Responses;
            Assert.Equal(new[] { expectedStatusCode }, responses.Keys.ToArray());
            var response = responses[expectedStatusCode];
            Assert.Equal(expectedDescriptions, response.Description);
            Assert.Equal(0, response.Content.Count);
        }

        [Theory]
        [InlineData(nameof(FakeActions.ReturnsEnumerable), "200", "Success")]
        [InlineData(nameof(FakeActions.ReturnsComplexType), "200", "Success")]
        [InlineData(nameof(FakeActions.ReturnsJObject), "200", "Success")]
        public void GetSwagger_GeneratesResponsesFromReturnTypes_IfResponseTypeAttributesNotPresent(
            string actionFixtureName,
            string expectedStatusCode,
            string expectedDescriptions)
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis =>
                apis.Add("GET", "collection", actionFixtureName));

            var swagger = subject.GetSwagger("v1");

            var responses = swagger.Paths["/collection"].Get.Responses;
            Assert.Equal(new[] { expectedStatusCode }, responses.Keys.ToArray());
            var response = responses[expectedStatusCode];
            Assert.Equal(expectedDescriptions, response.Description);
            var responseContent = response.Content["application/json"];
            Assert.NotNull(responseContent);
            Assert.NotNull(responseContent.Schema);
        }

        [Fact]
        public void GetSwagger_GeneratesResponsesFromResponseTypeAttributes_IfResponseTypeAttributesPresent()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis =>
                apis.Add("GET", "collection", nameof(FakeActions.AnnotatedWithResponseTypeAttributes)));

            var swagger = subject.GetSwagger("v1");

            var responses = swagger.Paths["/collection"].Get.Responses;
            Assert.Equal(new[] { "204", "400" }, responses.Keys.ToArray());
            var response1 = responses["204"];
            Assert.Equal("Success", response1.Description);
            Assert.Equal(0, response1.Content.Count);

            var response2 = responses["400"];
            Assert.Equal("Bad Request", response2.Description);
            var responseContent2 = response2.Content["application/json"];
            Assert.NotNull(responseContent2);
            Assert.NotNull(responseContent2.Schema);
        }

        [Fact(Skip = "Not implemented yet")]
        public void GetSwagger_GeneratesResponsesFromSwaggerResponseAttributes_IfResponseAttributesPresent()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis =>
                apis.Add("GET", "collection", nameof(FakeActions.AnnotatedWithSwaggerResponseAttributes)));

            var swagger = subject.GetSwagger("v1");

            var responses = swagger.Paths["/collection"].Get.Responses;
            Assert.Equal(new[] { "204", "400" }, responses.Keys.ToArray());
            var response1 = responses["204"];
            Assert.Equal("No content is returned.", response1.Description);
            Assert.Equal(0, response1.Content.Count);

            var response2 = responses["400"];
            Assert.Equal("This returns a dictionary.", response2.Description);
            var responseContent2 = response2.Content["application/json"];
            Assert.NotNull(responseContent2);
            Assert.NotNull(responseContent2.Schema);
        }
    }
}