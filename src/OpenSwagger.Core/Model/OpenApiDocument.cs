using System.Collections;
using System.Collections.Generic;

namespace OpenSwagger.Core.Model
{
    public sealed class OpenApiDocument
    {
        /// <summary>
        /// This string MUST be the semantic version number of the OpenAPI Specification version that the OpenAPI document uses.
        /// </summary>
        public string Openapi { get; } = "3.0.0";

        /// <summary>
        /// Provides metadata about the API.
        /// </summary>
        public Info Info { get; set; }

        /// <summary>
        /// An array of Server Objects, which provide connectivity information to a target server. 
        /// </summary>
        public IEnumerable<Server> Servers { get; set; }

        /// <summary>
        /// The available paths and operations for the API.
        /// </summary>
        public IDictionary<string, PathItem> Paths { get; set; }

        /// <summary>
        /// An element to hold various schemas for the specification.
        /// </summary>
        public Components Components { get; set; }

        /// <summary>
        /// A declaration of which security mechanisms can be used across the API.
        /// The list of values includes alternative security requirement objects that can be used. 
        /// </summary>
        public ICollection<IDictionary<string, IEnumerable<string>>> Security { get; set; } = new List<IDictionary<string, IEnumerable<string>>>();

        /// <summary>
        /// A list of tags used by the specification with additional metadata.
        /// </summary>
        public IEnumerable<Tag> Tags { get; set; }

        /// <summary>
        /// Additional external documentation.
        /// </summary>
        public IEnumerable<ExternalDocs> ExternalDocs { get; set; }
    }
}
