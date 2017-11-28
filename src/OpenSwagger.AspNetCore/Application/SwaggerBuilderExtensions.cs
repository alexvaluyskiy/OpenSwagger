using System;
using OpenSwagger.AspNetCore.Application;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerBuilderExtensions
    {
        public static IApplicationBuilder UseOpenSwagger(
            this IApplicationBuilder app,
            Action<SwaggerOptions> setupAction = null)
        {
            var options = new SwaggerOptions();
            setupAction?.Invoke(options);

            return app.UseMiddleware<SwaggerMiddleware>(options);
        }
    }
}