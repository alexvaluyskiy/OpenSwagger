namespace OpenSwagger.Core.Model
{
    public sealed class ApiKeySecurityScheme : SecurityScheme
    {
        public ApiKeySecurityScheme()
        {
            Type = "apiKey";
        }

        /// <summary>
        /// The name of the header, query or cookie parameter to be used.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The location of the API key. Valid values are "query", "header" or "cookie".
        /// </summary>
        public string In { get; set; }
    }
}