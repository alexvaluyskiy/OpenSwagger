﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenSwagger.AspNetCore.ApiExplorer;
using OpenSwagger.AspNetCore.Application;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerGenServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenSwagger(
            this IServiceCollection services,
            Action<OpenSwaggerOptions> setupAction)
        {
            services.Configure<MvcOptions>(c =>
                c.Conventions.Add(new SwaggerApplicationConvention()));

            services.Configure(setupAction ?? (opts => { }));

            services.AddTransient(CreateSwaggerProvider);

            return services;
        }

        public static void ConfigureSwaggerGen(
            this IServiceCollection services,
            Action<OpenSwaggerOptions> setupAction)
        {
            services.Configure(setupAction);
        }

        private static ISwaggerProvider CreateSwaggerProvider(IServiceProvider serviceProvider)
        {
            var swaggerGenOptions = serviceProvider.GetRequiredService<IOptions<OpenSwaggerOptions>>().Value;
            return swaggerGenOptions.CreateSwaggerProvider(serviceProvider);
        }
    }
}
