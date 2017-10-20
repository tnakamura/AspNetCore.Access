using Microsoft.AspNetCore.Http;

namespace AspNetCore.Contrib.Access
{
    /// <summary>
    /// Defines a contract for an access mapping builder in an application.
    /// </summary>
    public interface IAccessMappingBuilder
    {
        IAccessMappingBuilder MapPath(PathString path, string ipAddress, params string[] optionalIpAddresses);

        /// <summary>
        /// Builds an <see cref="AccessOptions"/>
        /// </summary>
        /// <returns></returns>
        AccessOptions Build();
    }
}
