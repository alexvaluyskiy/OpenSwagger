namespace OpenSwagger.Core.Model
{
    public sealed class License
    {
        /// <summary>
        /// The license name used for the API.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A URL to the license used for the API. MUST be in the format of a URL.
        /// </summary>
        public string Url { get; set; }
    }
}