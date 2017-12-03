namespace OpenSwagger.Core.Model
{
    public sealed class Info
    {
        /// <summary>
        /// The version of the OpenAPI document (which is distinct from the OpenAPI Specification version or the API implementation version).
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The title of the application.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short description of the application.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// A URL to the Terms of Service for the API. MUST be in the format of a URL.
        /// </summary>
        public string TermsOfService { get; set; }

        /// <summary>
        /// The contact information for the exposed API.
        /// </summary>
        public Contact Contact { get; set; }

        /// <summary>
        /// The license information for the exposed API.
        /// </summary>
        public License License { get; set; }
    }
}