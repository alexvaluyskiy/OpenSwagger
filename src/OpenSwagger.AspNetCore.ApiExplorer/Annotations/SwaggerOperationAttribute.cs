using System;

namespace OpenSwagger.AspNetCore.ApiExplorer.Annotations
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SwaggerOperationAttribute : Attribute
    {
        public SwaggerOperationAttribute(string operationId = null)
        {
            OperationId = operationId;
        }

        public string OperationId { get; set; }

        public string[] Tags { get; set; }

        // TODO: remove it
        public string[] Schemes { get; set; }
    }
}