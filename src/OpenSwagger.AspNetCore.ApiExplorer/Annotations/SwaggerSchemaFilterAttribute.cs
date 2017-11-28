﻿using System;

namespace OpenSwagger.AspNetCore.ApiExplorer.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class SwaggerSchemaFilterAttribute : Attribute
    {
        public SwaggerSchemaFilterAttribute(Type type)
        {
            Type = type;
            Arguments = new object[]{ };
        }

        public Type Type { get; private set; }

        public object[] Arguments { get; set; }
    }
}