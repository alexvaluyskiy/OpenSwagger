using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using OpenSwagger.Core.Model;
using System.IO;
using Basic.Swagger;

namespace Basic
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv;

        public Startup(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        // This method gets called by a runtime.
        // Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            // Uncomment the following line to add Web API services which makes it easier to port Web API 2 controllers.
            // You will also need to add the Microsoft.AspNetCore.Mvc.WebApiCompatShim package to the 'dependencies' section of project.json.
            // services.AddWebApiConventions();

            services.AddOpenSwagger(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Version = "v1",
                        Title = "OpenSwagger Sample API",
                        Description = "A sample API for testing OpenSwagger",
                        TermsOfService = "Some terms ..."
                    }
                );

                //c.OperationFilter<AssignOperationVendorExtensions>();
                //c.OperationFilter<FormDataOperationFilter>();

                //c.DescribeAllParametersInCamelCase();

                c.AddSecurityDefinition("OAuth2", new OAuth2SecurityScheme
                {
                    Description = "OAuth2 Authorization Code Grant",
                    Flows = new Dictionary<string, OAuth2SecurityScheme.OAuth2Flow>
                    {
                        ["authorizationCode"] = new OAuth2SecurityScheme.AuthorizationCode
                        {
                            AuthorizationUrl = "https://tempuri.org/auth",
                            TokenUrl = "https://tempuri.org/token",
                            Scopes = new Dictionary<string, string>
                            {
                                ["read"] = "Read access to protected resources",
                                ["write"] = "Write access to protected resources",
                            },
                        }
                    }
                });

                c.DocumentFilter<GlobalSecurityDocumentFilter>();
            });

            if (_hostingEnv.IsDevelopment())
            {
                services.ConfigureSwaggerGen(c =>
                {
                    var xmlCommentsPath = Path.Combine(System.AppContext.BaseDirectory, "Basic.xml");
                    c.IncludeXmlComments(xmlCommentsPath);
                });
            }
        }

        // Configure is called after ConfigureServices is called.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            // Add MVC to the request pipeline.
            app.UseDeveloperExceptionPage();
            app.UseMvc();
            // Add the following route for porting Web API 2 controllers.
            // routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");

            app.UseOpenSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            });
        }
    }
}
