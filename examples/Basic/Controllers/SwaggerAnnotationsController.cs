using Microsoft.AspNetCore.Mvc;
using Basic.Swagger;
using OpenSwagger.AspNetCore.ApiExplorer.Annotations;

namespace Basic.Controllers
{
    public class SwaggerAnnotationsController
    {
        [SwaggerOperation("CreateCart", Tags = new []{ "a", "b" })]
        [HttpPost("/carts")]
        public Cart Create([FromBody]Cart cart)
        {
            return new Cart { Id = 1 };
        }

        [HttpGet("/carts/docs")]
        [SwaggerOperationFilter(typeof(OperationExternalDocsOperationFilter))]
        public Cart GetById(int id)
        {
            return new Cart { Id = id };
        }

        [HttpGet("/carts/callbacks")]
        [SwaggerOperationFilter(typeof(OperationCallbacksOperationFilter))]
        public string Callbacks(int id)
        {
            return "callbacks";
        }

        [HttpGet("/carts/servers")]
        [SwaggerOperationFilter(typeof(OperationServersOperationFilter))]
        public string Servers(int id)
        {
            return "servers";
        }

        [HttpGet("/carts/examples")]
        [SwaggerOperationFilter(typeof(ResponseExamplesOperationFilter))]
        public string Example(int id)
        {
            return "examples";
        }

        [HttpGet("/carts/headers")]
        [SwaggerOperationFilter(typeof(ResponseHeadersOperationFilter))]
        public string Headers(int id)
        {
            return "headers";
        }

        [HttpGet("/carts/links")]
        [SwaggerOperationFilter(typeof(ResponseLinksOperationFilter))]
        public string Links(int id)
        {
            return "links";
        }
    }

    [SwaggerSchemaFilter(typeof(AddCartDefault))]
    public class Cart
    {
        public int Id { get; internal set; }
    }
}