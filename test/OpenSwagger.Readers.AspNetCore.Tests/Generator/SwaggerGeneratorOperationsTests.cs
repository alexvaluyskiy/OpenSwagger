 using FluentAssertions;
 using Microsoft.OpenApi.Models;
 using OpenSwagger.AspNetCore.ApiExplorer.Tests;
using Xunit;

namespace OpenSwagger.Readers.AspNetCore.Tests.Generator
{
    public class SwaggerGeneratorOperationsTests
    {
        [Theory]
        [InlineData("api/products", "ApiProductsGet")]
        [InlineData("addresses/validate", "AddressesValidateGet")]
        [InlineData("carts/{cartId}/items/{id}", "CartsByCartIdItemsByIdGet")]
        public void GetSwagger_GeneratesOperationIds_AccordingToRouteTemplateAndHttpMethod(
            string routeTemplate,
            string expectedOperationId
        )
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("GET", routeTemplate, nameof(FakeActions.AcceptsNothing)));

            swagger.Paths["/" + routeTemplate].Operations[OperationType.Get].OperationId.Should()
                .Be(expectedOperationId);
        }

        [Fact]
        public void GetSwagger_SetsDeprecated_IfActionsMarkedObsolete()
        {
            var swagger = SwaggerTestHelper.GenerateDocument(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.MarkedObsolete)));

            swagger.Paths["/collection"].Operations[OperationType.Get].Deprecated.Should().BeTrue();
        }
    }
}