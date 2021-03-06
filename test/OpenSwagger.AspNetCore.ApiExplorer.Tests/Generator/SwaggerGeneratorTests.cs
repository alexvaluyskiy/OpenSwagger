﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OpenSwagger.Core.Model;
using Xunit;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.Generator
{
    public class SwaggerGeneratorTests
    {
        [Fact]
        public void GetSwagger_RequiresTargetDocumentToBeSpecifiedBySettings()
        {
            var subject = SwaggerTestHelper.Subject(configure: (c) => c.SwaggerDocs.Clear());

            Assert.Throws<UnknownSwaggerDocument>(() => subject.GetSwagger("v1"));
        }

        [Fact]
        public void GetSwagger_GeneratesOneOrMoreDocuments_AsSpecifiedBySettings()
        {
            var v1Info = new Info { Version = "v2", Title = "API V2" };
            var v2Info = new Info { Version = "v1", Title = "API V1" };

            var subject = SwaggerTestHelper.Subject(
                setupApis: apis =>
                {
                    apis.Add("GET", "v1/collection", nameof(FakeActions.ReturnsEnumerable));
                    apis.Add("GET", "v2/collection", nameof(FakeActions.ReturnsEnumerable));
                },
                configure: c =>
                {
                    c.SwaggerDocs.Clear();
                    c.SwaggerDocs.Add("v1", v1Info);
                    c.SwaggerDocs.Add("v2", v2Info);
                    c.DocInclusionPredicate = (docName, api) => api.RelativePath.StartsWith(docName);
                });

            var v1Swagger = subject.GetSwagger("v1");
            var v2Swagger = subject.GetSwagger("v2");

            Assert.Equal(new[] { "/v1/collection" }, v1Swagger.Paths.Keys.ToArray());
            Assert.Equal(v1Info, v1Swagger.Info);
            Assert.Equal(new[] { "/v2/collection" }, v2Swagger.Paths.Keys.ToArray());
            Assert.Equal(v2Info, v2Swagger.Info);
        }

        [Fact]
        public void GetSwagger_GeneratesPathItem_PerRelativePathSansQueryString()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", "collection1", nameof(FakeActions.ReturnsEnumerable))
                .Add("GET", "collection1/{id}", nameof(FakeActions.ReturnsComplexType))
                .Add("GET", "collection2", nameof(FakeActions.AcceptsStringFromQuery))
                .Add("PUT", "collection2", nameof(FakeActions.ReturnsVoid))
                .Add("GET", "collection2/{id}", nameof(FakeActions.ReturnsComplexType))
            );

            var swagger = subject.GetSwagger("v1");

            Assert.Equal(new[]
                {
                    "/collection1",
                    "/collection1/{id}",
                    "/collection2",
                    "/collection2/{id}"
                },
                swagger.Paths.Keys.ToArray());
        }

        //[Fact]
        //public void GetSwagger_GeneratesOperation_PerHttpMethodPerRelativePathSansQueryString()
        //{
        //    var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
        //        .Add("GET", "collection", nameof(FakeActions.ReturnsEnumerable))
        //        .Add("PUT", "collection/{id}", nameof(FakeActions.AcceptsComplexTypeFromBody))
        //        .Add("POST", "collection", nameof(FakeActions.AcceptsComplexTypeFromBody))
        //        .Add("DELETE", "collection/{id}", nameof(FakeActions.ReturnsVoid))
        //        .Add("PATCH", "collection/{id}", nameof(FakeActions.AcceptsComplexTypeFromBody))
        //    // TODO: OPTIONS & HEAD
        //    );

        //    var swagger = subject.GetSwagger("v1");

        //    // GET collection
        //    var operation = swagger.Paths["/collection"].Get;
        //    Assert.NotNull(operation);
        //    Assert.Empty(operation.Parameters.Consumes);
        //    Assert.Equal(new[] { "application/json", "text/json" }, operation.Produces.ToArray());
        //    Assert.Null(operation.Deprecated);
        //    // PUT collection/{id}
        //    operation = swagger.Paths["/collection/{id}"].Put;
        //    Assert.NotNull(operation);
        //    Assert.Equal(new[] { "application/json", "text/json", "application/*+json" }, operation.Consumes.ToArray());
        //    Assert.Empty(operation.Produces.ToArray());
        //    Assert.Null(operation.Deprecated);
        //    // POST collection
        //    operation = swagger.Paths["/collection"].Post;
        //    Assert.NotNull(operation);
        //    Assert.Equal(new[] { "application/json", "text/json", "application/*+json" }, operation.Consumes.ToArray());
        //    Assert.Empty(operation.Produces.ToArray());
        //    Assert.Null(operation.Deprecated);
        //    // DELETE collection/{id}
        //    operation = swagger.Paths["/collection/{id}"].Delete;
        //    Assert.NotNull(operation);
        //    Assert.Empty(operation.Consumes.ToArray());
        //    Assert.Empty(operation.Produces.ToArray());
        //    Assert.Null(operation.Deprecated);
        //    // PATCH collection
        //    operation = swagger.Paths["/collection/{id}"].Patch;
        //    Assert.NotNull(operation);
        //    Assert.Equal(new[] { "application/json", "text/json", "application/*+json" }, operation.Consumes.ToArray());
        //    Assert.Empty(operation.Produces.ToArray());
        //    Assert.Null(operation.Deprecated);
        //}

        [Fact]
        public void GetSwagger_IgnoresObsoleteActions_IfSpecifiedBySettings()
        {
            var subject = SwaggerTestHelper.Subject(
                setupApis: apis =>
                {
                    apis.Add("GET", "collection1", nameof(FakeActions.ReturnsEnumerable));
                    apis.Add("GET", "collection2", nameof(FakeActions.MarkedObsolete));
                },
                configure: c => c.IgnoreObsoleteActions = true);

            var swagger = subject.GetSwagger("v1");

            Assert.Equal(new[] { "/collection1" }, swagger.Paths.Keys.ToArray());
        }

        [Fact]
        public void GetSwagger_OrdersActions_AsSpecifiedBySettings()
        {
            var subject = SwaggerTestHelper.Subject(
                setupApis: apis =>
                {
                    apis.Add("GET", "B", nameof(FakeActions.ReturnsVoid));
                    apis.Add("GET", "A", nameof(FakeActions.ReturnsVoid));
                    apis.Add("GET", "F", nameof(FakeActions.ReturnsVoid));
                    apis.Add("GET", "D", nameof(FakeActions.ReturnsVoid));
                },
                configure: c =>
                {
                    c.SortKeySelector = (apiDesc) => apiDesc.RelativePath;
                });

            var swagger = subject.GetSwagger("v1");

            Assert.Equal(new[] { "/A", "/B", "/D", "/F" }, swagger.Paths.Keys.ToArray());
        }

        //[Fact]
        //public void GetSwagger_ExecutesOperationFilters_IfSpecifiedBySettings()
        //{
        //    var subject = SwaggerTestHelper.Subject(
        //        setupApis: apis =>
        //        {
        //            apis.Add("GET", "collection", nameof(FakeActions.ReturnsEnumerable));
        //        },
        //        configure: c =>
        //        {
        //            c.OperationFilters.Add(new VendorExtensionsOperationFilter());
        //        });

        //    var swagger = subject.GetSwagger("v1");

        //    var operation = swagger.Paths["/collection"].Get;
        //    Assert.NotEmpty(operation.Extensions);
        //}

        //[Fact]
        //public void GetSwagger_ExecutesDocumentFilters_IfSpecifiedBySettings()
        //{
        //    var subject = SwaggerTestHelper.Subject(configure: opts =>
        //        opts.DocumentFilters.Add(new VendorExtensionsDocumentFilter()));

        //    var swagger = subject.GetSwagger("v1");

        //    Assert.NotEmpty(swagger.Extensions);
        //}

        [Fact]
        public void GetSwagger_HandlesUnboundRouteParams()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", "{version}/collection", nameof(FakeActions.AcceptsNothing)));

            var swagger = subject.GetSwagger("v1");

            var param = swagger.Paths["/{version}/collection"].Get.Parameters.First();
            Assert.Equal("version", param.Name);
            Assert.Equal(true, param.Required);
        }

        [Fact]
        public void GetSwagger_ThrowsInformativeException_IfHttpMethodAttributeNotPresent()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add(null, "collection", nameof(FakeActions.AcceptsNothing)));

            var exception = Assert.Throws<NotSupportedException>(() => subject.GetSwagger("v1"));
            Assert.Equal(
                "Ambiguous HTTP method for action - OpenSwagger.AspNetCore.ApiExplorer.Tests.FakeControllers+NotAnnotated.AcceptsNothing (OpenSwagger.AspNetCore.ApiExplorer.Tests). " +
                "Actions require an explicit HttpMethod binding for Swagger",
                exception.Message);
        }

        [Fact]
        public void GetSwagger_ThrowsInformativeException_IfHttpMethodAndPathAreOverloaded()
        {
            var subject = SwaggerTestHelper.Subject(setupApis: apis => apis
                .Add("GET", "collection", nameof(FakeActions.AcceptsNothing))
                .Add("GET", "collection", nameof(FakeActions.AcceptsStringFromQuery))
            );

            var exception = Assert.Throws<NotSupportedException>(() => subject.GetSwagger("v1"));
            Assert.Equal(
                "HTTP method \"GET\" & path \"collection\" overloaded by actions - " +
                "OpenSwagger.AspNetCore.ApiExplorer.Tests.FakeControllers+NotAnnotated.AcceptsNothing (OpenSwagger.AspNetCore.ApiExplorer.Tests)," +
                "OpenSwagger.AspNetCore.ApiExplorer.Tests.FakeControllers+NotAnnotated.AcceptsStringFromQuery (OpenSwagger.AspNetCore.ApiExplorer.Tests). " +
                "Actions require unique method/path combination for Swagger",
                exception.Message);
        }
    }
}