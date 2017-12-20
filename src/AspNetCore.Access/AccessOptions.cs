using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;

namespace AspNetCore.Access
{
    /// <summary>
    /// Provides programmatic configuration for the Access middleware.
    /// </summary>
    public class AccessOptions
    {
        /// <summary>
        /// Create a new instance of <see cref="AccessOptions"/>
        /// </summary>
        /// <param name="mappings">
        /// The mappings configured in the <see cref="IAccessMappingBuilder"/>.
        /// </param>
        public AccessOptions(ILookup<PathString, IPAddress> mappings)
        {
            Mappings = mappings;
        }

        /// <summary>
        /// Gets the mappings configured in the <see cref="IAccessMappingBuilder"/>.
        /// </summary>
        public ILookup<PathString, IPAddress> Mappings { get; }
    }
}
