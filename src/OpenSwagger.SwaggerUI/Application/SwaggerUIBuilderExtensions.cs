﻿using System;
using Microsoft.AspNetCore.StaticFiles;
using OpenSwagger.SwaggerUI.Application;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerUIBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerUI(
            this IApplicationBuilder app,
            Action<SwaggerUI3Options> setupAction)
        {
            var options = new SwaggerUI3Options();
            setupAction?.Invoke(options);

            // Serve swagger-ui assets with the FileServer middleware, using a custom FileProvider
            // to inject parameters into "index.html"
            var fileServerOptions = new FileServerOptions
            {
                RequestPath = $"/{options.RoutePrefix}",
                FileProvider = new SwaggerUIFileProvider(options.IndexSettings.ToTemplateParameters()),
                EnableDefaultFiles = true, // serve index.html at /{options.RoutePrefix}/
            };
            fileServerOptions.StaticFileOptions.ContentTypeProvider = new FileExtensionContentTypeProvider();
            app.UseFileServer(fileServerOptions);

            return app;
        }
    }
}
