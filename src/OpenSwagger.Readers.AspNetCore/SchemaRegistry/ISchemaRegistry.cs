using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace OpenSwagger.Readers.AspNetCore
{
    public interface ISchemaRegistry
    {
        OpenApiSchema GetOrRegister(Type type);

        IDictionary<string, OpenApiSchema> Definitions { get; }
    }
}
