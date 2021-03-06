﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.XPath;
using Newtonsoft.Json.Serialization;
using OpenSwagger.AspNetCore.ApiExplorer.Annotations;
using OpenSwagger.Core.Model;
using Xunit;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.Annotations
{
    public class XmlCommentsSchemaFilterTests
    {
        [Theory]
        [InlineData(typeof(XmlAnnotatedType), "summary for XmlAnnotatedType", Skip = "Not implemented yet")]
        [InlineData(typeof(XmlAnnotatedWithNestedType.NestedType), "summary for NestedType", Skip = "Not implemented yet")]
        [InlineData(typeof(XmlAnnotatedGenericType<string>), "summary for XmlAnnotatedGenericType", Skip = "Not implemented yet")]
        public void Apply_SetsDescription_FromClassSummaryTag(
            Type type,
            string expectedDescription)
        {
            var schema = new Schema
            {
                Properties = new Dictionary<string, Schema>()
            };
            var filterContext = FilterContextFor(type);

            Subject().Apply(schema, filterContext);

            Assert.Equal(expectedDescription, schema.Description);
        }

        [Theory]
        [InlineData(typeof(XmlAnnotatedType), "Property", "summary for Property", Skip = "Not implemented yet")]
        [InlineData(typeof(XmlAnnotatedSubType), "BaseProperty", "summary for BaseProperty", Skip = "Not implemented yet")]
        [InlineData(typeof(XmlAnnotatedGenericType<string>), "GenericProperty", "summary for GenericProperty", Skip = "Not implemented yet")]
        public void Apply_SetsPropertyDescriptions_FromPropertySummaryTag(
            Type type,
            string propertyName,
            string expectedDescription)
        {
            var schema = new Schema
            {
                Properties = new Dictionary<string, Schema>()
                {
                    { propertyName, new Schema() }
                }
            };
            var filterContext = FilterContextFor(type);

            Subject().Apply(schema, filterContext);

            Assert.Equal(expectedDescription, schema.Properties[propertyName].Description);
        }

        private SchemaFilterContext FilterContextFor(Type type)
        {
            var jsonObjectContract = new DefaultContractResolver().ResolveContract(type);
            return new SchemaFilterContext(type, (jsonObjectContract as JsonObjectContract), null);
        }

        private XmlCommentsSchemaFilter Subject()
        {
            var xmlComments = GetType().GetTypeInfo()
                .Assembly
                .GetManifestResourceStream("OpenSwagger.AspNetCore.ApiExplorer.Tests.XmlComments.xml");

            return new XmlCommentsSchemaFilter(new XPathDocument(xmlComments));
        }
    }
}