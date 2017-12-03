using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class OAuth2SecurityScheme : SecurityScheme
    {
        public abstract class OAuth2Flow
        {
            /// <summary>
            /// The URL to be used for obtaining refresh tokens. This MUST be in the form of a URL.
            /// </summary>
            public string RefreshUrl { get; set; }

            /// <summary>
            /// The available scopes for the OAuth2 security scheme.
            /// A map between the scope name and a short description for it.
            /// </summary>
            public IDictionary<string, string> Scopes { get; set; }
        }

        public class Implicit : OAuth2Flow
        {
            /// <summary>
            /// The authorization URL to be used for this flow. This MUST be in the form of a URL.
            /// </summary>
            public string AuthorizationUrl { get; set; }
        }

        public class AuthorizationCode : OAuth2Flow
        {
            /// <summary>
            /// The authorization URL to be used for this flow. This MUST be in the form of a URL.
            /// </summary>
            public string AuthorizationUrl { get; set; }

            /// <summary>
            /// The token URL to be used for this flow. This MUST be in the form of a URL.
            /// </summary>
            public string TokenUrl { get; set; }
        }

        public class ClientCredentials : OAuth2Flow
        {
            /// <summary>
            /// The token URL to be used for this flow. This MUST be in the form of a URL.
            /// </summary>
            public string TokenUrl { get; set; }
        }

        public class Password : OAuth2Flow
        {
            /// <summary>
            /// The token URL to be used for this flow. This MUST be in the form of a URL.
            /// </summary>
            public string TokenUrl { get; set; }
        }

        public OAuth2SecurityScheme()
        {
            Type = "oauth2";
        }

        /// <summary>
        /// An object containing configuration information for the flow types supported.
        /// </summary>
        public IDictionary<string, OAuth2Flow> Flows { get; set; }
    }
}