using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace OpenSwagger.Readers.AspNetCore.Conventions
{
    public class SwaggerApplicationConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            application.ApiExplorer.IsVisible = true;
        }
    }
}
