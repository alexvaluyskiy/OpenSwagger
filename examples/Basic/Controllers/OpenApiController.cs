using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using OpenSwagger.Readers.AspNetCore;

namespace Basic.Controllers
{
    [Route("/openapi")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class OpenApiController : Controller
    {
        private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;

        public OpenApiController(IApiDescriptionGroupCollectionProvider apiExplorer)
        {
            _apiExplorer = apiExplorer;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var settings = new ApiExplorerReaderSettings();
            var reader = new ApiExplorerReader(settings);
            var document = reader.Read(_apiExplorer, out var diagnostic);

            // info
            document.Info = new OpenApiInfo
            {
                Title = "OpenAPI",
                Description = "API documentation description",
                Version = "1.0",
                Contact = new OpenApiContact
                {
                    Name = "CompanyName",
                    Email = "company@gmail.com"
                },
            };

            if (diagnostic.Errors.Count > 0)
                return Ok(diagnostic.Errors);

            return Content(document.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json), "application/json");
        }
    }
}