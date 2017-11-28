using System;
using System.Collections.Generic;
using OpenSwagger.Core.Model;

namespace OpenSwagger.AspNetCore.ApiExplorer
{
    public interface ISchemaRegistry
    {
        Schema GetOrRegister(Type type);

        IDictionary<string, Schema> Definitions { get; }
    }
}
