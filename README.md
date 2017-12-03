# OpenSwagger.AspNetCore

[![Build status](https://ci.appveyor.com/api/projects/status/7834y6i56alqgi7y/branch/master?svg=true)](https://ci.appveyor.com/project/ravengerUA/openswagger/branch/master)

**OpenSwagger Core Features:**
- Auto-generated OpenAPI 3.0
- Seamless integration of swagger-ui
- Reflection-based Schema generation for describing API types
- Extensibility hooks for customizing the generated Swagger doc
- Extensibility hooks for customizing the swagger-ui
- Out-of-the-box support for leveraging Xml comments
- Support for describing ApiKey, Http Basic, Http Bearer, OAuth2 and OpenId Connect schemes

# Components #

OpenSwagger consists of four packages - a OpenAPI generator, middleware to expose the generated OpenAPI as JSON endpoints and middleware to expose a swagger-ui that's powered by those endpoints.See the table below for more details.

|Package|Description|
|---------|-----------|
|__OpenSwagger.Core__|Exposes _OpenApiDocument_ objects as a JSON API. It expects an implementation of _ISwaggerProvider_ to be registered which it queries to retrieve OpenAPI document(s) before returning as serialized JSON|
|__OpenSwagger.AspNetCore__|Injects an implementation of _ISwaggerProvider_ that can be used by the above component. This particular implementation automatically generates _OpenApiDocument_(s) from your routes, controllers and models|
|__OpenSwagger.SwaggerUI__|Exposes an embedded version of the swagger-ui. You specify the API endpoints where it can obtain OpenAPI JSON and it uses them to power interactive docs for your API|