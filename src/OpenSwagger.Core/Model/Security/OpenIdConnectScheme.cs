namespace OpenSwagger.Core.Model
{
    public sealed class OpenIdConnectScheme : SecurityScheme
    {
        public OpenIdConnectScheme()
        {
            Type = "openIdConnect";
        }

        /// <summary>
        /// OpenId Connect URL to discover OAuth2 configuration values. This MUST be in the form of a URL.
        /// </summary>
        public string OpenIdConnectUrl { get; set; }
    }
}