namespace OpenSwagger.Core.Model
{
    public abstract class SecurityScheme
    {
        /// <summary>
        /// The type of the security scheme
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// A short description for security scheme
        /// </summary>
        public string Description { get; set; }
    }
}
