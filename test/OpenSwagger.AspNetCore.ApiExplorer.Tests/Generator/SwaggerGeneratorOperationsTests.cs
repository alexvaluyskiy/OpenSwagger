using System;
using Newtonsoft.Json;
using OpenSwagger.Core.Model;
using Xunit;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.Generator
{
    public class SwaggerGeneratorOperationsTests
    {
        [Fact]
        public void GetSwagger_TagsActions_AsSpecifiedBySettings()
        {
            var subject = SwaggerTestHelper.Subject(
                setupApis: apis =>
                {
                    apis.Add("GET", "collection1", nameof(FakeActions.ReturnsEnumerable));
                    apis.Add("GET", "collection2", nameof(FakeActions.ReturnsInt));
                },
                configure: c => c.TagSelector = (apiDesc) => apiDesc.RelativePath);

            var swagger = subject.GetSwagger("v1");

            Assert.Equal(new[] { "collection1" }, swagger.Paths["/collection1"].Get.Tags);
            Assert.Equal(new[] { "collection2" }, swagger.Paths["/collection2"].Get.Tags);
        }

        [Theory]
        [InlineData("api/products", "ApiProductsGet")]
        [InlineData("addresses/validate", "AddressesValidateGet")]
        [InlineData("carts/{cartId}/items/{id}", "CartsByCartIdItemsByIdGet")]
        public void GetSwagger_GeneratesOperationIds_AccordingToRouteTemplateAndHttpMethod(
            string routeTemplate,
            string expectedOperationId
        )
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", routeTemplate, nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            Assert.Equal(expectedOperationId, swagger.Paths["/" + routeTemplate].Get.OperationId);
        }

        [Fact]
        public void GetSwagger_SetsDeprecated_IfActionsMarkedObsolete()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.MarkedObsolete)));

            var swagger = subject.GetSwagger("v1");

            var operation = swagger.Paths["/collection"].Get;
            Assert.True(operation.Deprecated);
        }
    }
}