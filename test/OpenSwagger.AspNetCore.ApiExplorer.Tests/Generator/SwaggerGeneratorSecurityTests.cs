using System;
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

            var subject = SwaggerTestHelper.Subject(configure: c =>
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

            var subject = SwaggerTestHelper.Subject(configure: c =>
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
                Description = "API Key Authentication",
                Name = "X-API-Key",
                In = "header"
            };

            var subject = SwaggerTestHelper.Subject(configure: c =>
            {
                c.SecurityDefinitions.Add(securitySchemeName, securityScheme);
                c.GlobalSecurity.Add(securitySchemeName, new List<string>());
            });

            var swagger = subject.GetSwagger("v1");
            swagger.Components.SecuritySchemes.Keys.Should().Contain(securitySchemeName);

            var scheme = swagger.Components.SecuritySchemes[securitySchemeName] as ApiKeySecurityScheme;
            scheme.Should().NotBeNull();
            scheme.Type.Should().Be(securityScheme.Type);
            scheme.Description.Should().Be(securityScheme.Description);
            scheme.Name.Should().Be(securityScheme.Name);
            scheme.In.Should().Be(securityScheme.In);

            swagger.Security.Should().ContainKey(securitySchemeName);
            swagger.Security[securitySchemeName].Should().HaveCount(0);
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

            var subject = SwaggerTestHelper.Subject(configure: c =>
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

            var subject = SwaggerTestHelper.Subject(configure: c =>
                c.SecurityDefinitions.Add(securitySchemeName, securityScheme));

            var swagger = subject.GetSwagger("v1");
            swagger.Components.SecuritySchemes.Keys.Should().Contain(securitySchemeName);

            var scheme = swagger.Components.SecuritySchemes[securitySchemeName] as OpenIdConnectScheme;
            scheme.Should().NotBeNull();
            scheme.Type.Should().Be("openIdConnect");
            scheme.Description.Should().Be(securityScheme.Description);
            scheme.OpenIdConnectUrl.Should().Be(securityScheme.OpenIdConnectUrl);
        }

        [Fact(Skip = "Security scheme matching is not implemented yet")]
        public void GetSwagger_SkipGenerateGlobalSecurity_IfNoSecuritySchemeMatched()
        {
            var securitySchemeName = "ApiKeyAuth";
            var securityScheme = new ApiKeySecurityScheme
            {
                Description = "API Key Authentication",
                Name = "X-API-Key",
                In = "header"
            };

            var subject = SwaggerTestHelper.Subject(configure: c =>
            {
                c.SecurityDefinitions.Add(securitySchemeName, securityScheme);
                c.GlobalSecurity.Add("ApiKeyAuth", new List<string>());
            });

            var swagger = subject.GetSwagger("v1");
            swagger.Components.SecuritySchemes.Keys.Should().Contain(securitySchemeName);
            swagger.Security.Should().BeEmpty();
        }

        [Fact(Skip = "Security scheme's scopes matching is not implemented yet")]
        public void GetSwagger_SkipGenerateGlobalSecurityScopes_IfNoScopesMatched()
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
                        },
                    }
                }
            };

            var subject = SwaggerTestHelper.Subject(configure: c =>
            {
                c.SecurityDefinitions.Add(securitySchemeName, securityScheme);
                c.GlobalSecurity.Add(securitySchemeName, new List<string>
                {
                    "write"
                });
            });

            var swagger = subject.GetSwagger("v1");
            swagger.Components.SecuritySchemes.Keys.Should().Contain(securitySchemeName);
            swagger.Security.Should().ContainKey(securitySchemeName);
            swagger.Security[securitySchemeName].Should().BeEmpty();
        }
    }
}