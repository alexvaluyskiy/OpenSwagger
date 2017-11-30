﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using OpenSwagger.Core.Model;
using Xunit;

namespace OpenSwagger.AspNetCore.ApiExplorer.Tests.Generator
{
    public class SwaggerGeneratorSecurityTests
    {
        [Fact]
        public void GetSwagger_GeneratesBasicAuthSecurityDefinition_IfSpecifiedBySettings()
        {
            var securitySchemeName = "BasicAuth";
            var securityScheme = new HttpSecurityScheme
            {
                Scheme = "basic",
                Description = "Basic HTTP Authentication",
            };

            var subject = Subject(configure: c =>
                c.SecurityDefinitions.Add(securitySchemeName, securityScheme));

            var swagger = subject.GetSwagger("v1");
            swagger.Components.SecuritySchemes.Keys.Should().Contain(securitySchemeName);

            var scheme = swagger.Components.SecuritySchemes[securitySchemeName] as HttpSecurityScheme;
            scheme.Should().NotBeNull();
            scheme.BearerFormat.Should().BeNullOrEmpty();
            scheme.Type.Should().Be("http");
            scheme.Scheme.Should().Be(securityScheme.Scheme);
            scheme.Description.Should().Be(securityScheme.Description);
        }

        [Fact]
        public void GetSwagger_GeneratesBearerAuthSecurityDefinition_IfSpecifiedBySettings()
        {
            var securitySchemeName = "BearerAuth";
            var securityScheme = new HttpSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Bearer HTTP Authentication",
            };

            var subject = Subject(configure: c =>
                c.SecurityDefinitions.Add(securitySchemeName, securityScheme));

            var swagger = subject.GetSwagger("v1");
            swagger.Components.SecuritySchemes.Keys.Should().Contain(securitySchemeName);

            var scheme = swagger.Components.SecuritySchemes[securitySchemeName] as HttpSecurityScheme;
            scheme.Should().NotBeNull();
            scheme.Type.Should().Be("http");
            scheme.Scheme.Should().Be(securityScheme.Scheme);
            scheme.BearerFormat.Should().Be(securityScheme.BearerFormat);
            scheme.Description.Should().Be(securityScheme.Description);
        }

        [Fact]
        public void GetSwagger_GeneratesApiKeySecurityDefinition_IfSpecifiedBySettings()
        {
            var securitySchemeName = "ApiKeyAuth";
            var securityScheme = new ApiKeySecurityScheme
            {
                Type = "apiKey",
                Description = "API Key Authentication",
                Name = "X-API-Key",
                In = "header"
            };

            var subject = Subject(configure: c =>
                c.SecurityDefinitions.Add(securitySchemeName, securityScheme));

            var swagger = subject.GetSwagger("v1");
            swagger.Components.SecuritySchemes.Keys.Should().Contain(securitySchemeName);

            var scheme = swagger.Components.SecuritySchemes[securitySchemeName] as ApiKeySecurityScheme;
            scheme.Should().NotBeNull();
            scheme.Type.Should().Be(securityScheme.Type);
            scheme.Description.Should().Be(securityScheme.Description);
            scheme.Name.Should().Be(securityScheme.Name);
            scheme.In.Should().Be(securityScheme.In);
        }

        [Fact]
        public void GetSwagger_GeneratesOAuth2SecurityDefinition_IfSpecifiedBySettings()
        {
            var securitySchemeName = "OAuth2";
            var securityScheme = new OAuth2SecurityScheme
            {
                Description = "OAuth2 Authorization Code Grant",
                Flow = new Dictionary<string, OAuth2SecurityScheme.OAuth2Flow>
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
            };

            var subject = Subject(configure: c =>
            {
                c.SecurityDefinitions.Add(securitySchemeName, securityScheme);
            });

            var swagger = subject.GetSwagger("v1");
            swagger.Components.SecuritySchemes.Keys.Should().Contain(securitySchemeName);

            var scheme = swagger.Components.SecuritySchemes[securitySchemeName] as OAuth2SecurityScheme;
            scheme.Should().NotBeNull();
            scheme.Type.Should().Be(securityScheme.Type);
            scheme.Description.Should().Be(securityScheme.Description);
            scheme.Flow.Should().ContainKey("authorizationCode");
            scheme.Flow["authorizationCode"].Should().BeOfType<OAuth2SecurityScheme.AuthorizationCode>();

            var authorizationCode = (OAuth2SecurityScheme.AuthorizationCode)scheme.Flow["authorizationCode"];
            authorizationCode.AuthorizationUrl.Should().Be("https://tempuri.org/auth");
            authorizationCode.TokenUrl.Should().Be("https://tempuri.org/token");
            authorizationCode.Scopes.Keys.Should().ContainInOrder("read", "write");
            authorizationCode.Scopes["read"].Should().Be("Read access to protected resources");
            authorizationCode.Scopes["write"].Should().Be("Write access to protected resources");
       }

        [Fact]
        public void GetSwagger_GeneratesOpenIdConnectSecurityDefinition_IfSpecifiedBySettings()
        {
            var securitySchemeName = "OpenID";
            var securityScheme = new OpenIdConnectScheme
            {
                Type = "openIdConnect",
                Description = "API Key Authentication",
                OpenIdConnectUrl = "https://example.com/.well-known/openid-configuration",
            };

            var subject = Subject(configure: c =>
                c.SecurityDefinitions.Add(securitySchemeName, securityScheme));

            var swagger = subject.GetSwagger("v1");
            swagger.Components.SecuritySchemes.Keys.Should().Contain(securitySchemeName);

            var scheme = swagger.Components.SecuritySchemes[securitySchemeName] as OpenIdConnectScheme;
            scheme.Should().NotBeNull();
            scheme.Type.Should().Be("openIdConnect");
            scheme.Description.Should().Be(securityScheme.Description);
            scheme.OpenIdConnectUrl.Should().Be(securityScheme.OpenIdConnectUrl);
        }

        private SwaggerGenerator Subject(
            Action<FakeApiDescriptionGroupCollectionProvider> setupApis = null,
            Action<SwaggerGeneratorSettings> configure = null)
        {
            var apiDescriptionsProvider = new FakeApiDescriptionGroupCollectionProvider();
            setupApis?.Invoke(apiDescriptionsProvider);

            var options = new SwaggerGeneratorSettings();
            options.SwaggerDocs.Add("v1", new Info { Title = "API", Version = "v1" });

            configure?.Invoke(options);

            return new SwaggerGenerator(
                apiDescriptionsProvider,
                new SchemaRegistryFactory(new JsonSerializerSettings(), new SchemaRegistrySettings()),
                options
            );
        }
    }
}