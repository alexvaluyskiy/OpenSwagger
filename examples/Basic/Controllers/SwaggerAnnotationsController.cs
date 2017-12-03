using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Basic.Swagger;
using OpenSwagger.AspNetCore.ApiExplorer.Annotations;

namespace Basic.Controllers
{
    public class SwaggerAnnotationsController : Controller
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

        [HttpGet("/carts/description")]
        [SwaggerResponse(200, Type = typeof(int), Description = "Cart received")]
        [SwaggerResponse(404, Type = typeof(IDictionary<string, string>), Description = "Cart not found")]
        public ActionResult Create(int id)
        {
            return Ok("description");
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

        [HttpGet("/carts/files")]
        [SwaggerOperationFilter(typeof(ResponseFilesOperationFilter))]
        public ActionResult Files(int id)
        {
            var file = new byte[] { 5, 6, 7, 8, 3 };
            return File(file, "application/octet-stream");
        }
    }

    [SwaggerSchemaFilter(typeof(AddCartDefault))]
    public class Cart
    {
        public int Id { get; internal set; }
    }
}