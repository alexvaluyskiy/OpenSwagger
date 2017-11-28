namespace OpenSwagger.AspNetCore.ApiExplorer.Tests
{
    public class FakeControllers
    {
        public class NotAnnotated
        {}

        //[SwaggerOperationFilter(typeof(VendorExtensionsOperationFilter))]
        //public class AnnotatedWithSwaggerOperationFilter
        //{ }

        public class TestController
        {}
    }
}