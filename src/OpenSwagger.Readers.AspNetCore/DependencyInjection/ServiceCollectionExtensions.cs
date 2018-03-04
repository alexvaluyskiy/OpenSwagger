using Microsoft.AspNetCore.Mvc;
using OpenSwagger.Readers.AspNetCore.Conventions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenSwagger(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(c => c.Conventions.Add(new SwaggerApplicationConvention()));

            return services;
        }
    }
}
