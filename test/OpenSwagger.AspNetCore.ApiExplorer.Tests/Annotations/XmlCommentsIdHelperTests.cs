using System;
using OpenSwagger.AspNetCore.ApiExplorer.Annotations;
using Xunit;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.Annotations
{
    public class XmlCommentsIdHelperTests
    {
        [Theory]
        [InlineData(nameof(FakeActions.AcceptsNothing), "M:OpenSwagger.AspNetCore.ApiExplorer.Tests.FakeActions.AcceptsNothing")]
        [InlineData(nameof(FakeActions.AcceptsNestedType), "M:OpenSwagger.AspNetCore.ApiExplorer.Tests.FakeActions.AcceptsNestedType(OpenSwagger.AspNetCore.ApiExplorer.Tests.ContainingType.NestedType)")]
        [InlineData(nameof(FakeActions.AcceptsGenericType), "M:OpenSwagger.AspNetCore.ApiExplorer.Tests.FakeActions.AcceptsGenericType(System.Collections.Generic.IEnumerable{System.String})")]
        [InlineData(nameof(FakeActions.AcceptsGenericGenericType), "M:OpenSwagger.AspNetCore.ApiExplorer.Tests.FakeActions.AcceptsGenericGenericType(System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.String,System.String}})")]
        [InlineData(nameof(FakeActions.AcceptsGenericArrayType), "M:OpenSwagger.AspNetCore.ApiExplorer.Tests.FakeActions.AcceptsGenericArrayType(System.Collections.Generic.KeyValuePair{System.String,System.String}[])")]
        public void GetCommentIdForMethod_ReturnsCorrectXmlCommentId_ForGivenMethodInfo(
            string actionFixtureName,
            string expectedCommentId
        )
        {
            var methodInfo = typeof(FakeActions).GetMethod(actionFixtureName);

            var commentId = XmlCommentsIdHelper.GetCommentIdForMethod(methodInfo);

            Assert.Equal(expectedCommentId, commentId);
        }

        [Theory]
        [InlineData(typeof(ContainingType.NestedType), "T:OpenSwagger.AspNetCore.ApiExplorer.Tests.ContainingType.NestedType")]
        [InlineData(typeof(XmlAnnotatedGenericType<>), "T:OpenSwagger.AspNetCore.ApiExplorer.Tests.XmlAnnotatedGenericType`1")]
        [InlineData(typeof(NoNamespaceType), "T:NoNamespaceType")]
        public void GetCommentIdForType_ReturnsCorrectXmlCommentId_ForGivenType(
            Type type,
            string expectedCommentId
        )
        {
            var commentId = XmlCommentsIdHelper.GetCommentIdForType(type);

            Assert.Equal(expectedCommentId, commentId);
        }

        [Theory]
        [InlineData(typeof(ContainingType.NestedType), nameof(ContainingType.NestedType.Property2), "P:OpenSwagger.AspNetCore.ApiExplorer.Tests.ContainingType.NestedType.Property2")]
        [InlineData(typeof(XmlAnnotatedGenericType<>), "GenericProperty", "P:OpenSwagger.AspNetCore.ApiExplorer.Tests.XmlAnnotatedGenericType`1.GenericProperty")]
        public void GetCommentIdForProperty_ReturnsCorrectXmlCommentId_ForGivenPropertyInfo(
            Type type,
            string propertyName,
            string expectedCommentId
        )
        {
            var propertyInfo = type.GetProperty(propertyName);

            var commentId = XmlCommentsIdHelper.GetCommentIdForProperty(propertyInfo);

            Assert.Equal(expectedCommentId, commentId);
        }
    }
}
