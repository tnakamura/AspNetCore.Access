using Microsoft.AspNetCore.Http;

namespace AspNetCore.Access
{
    /// <summary>
    /// Defines a contract for an access mapping builder in an application.
    /// </summary>
    public interface IAccessMappingBuilder
    {
        /// <summary>
        /// Adds a mapping to the <see cref="IAccessMappingBuilder"/>.
        /// </summary>
        /// <param name="path">The URL</param>
        /// <param name="ipAddress">Accept IP address</param>
        /// <param name="optionalIpAddresses">Accept IP addresses</param>
        /// <returns>
        /// A reference to this instance after the operation has completed.
        /// </returns>
        IAccessMappingBuilder MapPath(PathString path, string ipAddress, params string[] optionalIpAddresses);

        /// <summary>
        /// Builds an <see cref="AccessOptions"/>
        /// </summary>
        /// <returns>An <see cref="AccessOptions"/> instance.</returns>
        AccessOptions Build();
    }
}
