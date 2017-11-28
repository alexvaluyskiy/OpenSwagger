namespace OpenSwagger.Core.Model
{
    public sealed class HttpSecurityScheme : SecurityScheme
    {
        public HttpSecurityScheme()
        {
            Type = "http";
        }

        /// <summary>
        /// The name of the HTTP Authorization scheme to be used in the Authorization header.
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// A hint to the client to identify how the bearer token is formatted.
        /// Bearer tokens are usually generated by an authorization server, so this information is primarily for documentation purposes.
        /// </summary>
        public string BearerFormat { get; set; }
    }
}