using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Access
{
    class AccessMappingBuilder : IAccessMappingBuilder
    {
        readonly Dictionary<PathString, List<string>> mappings =
            new Dictionary<PathString, List<string>>();

        public AccessOptions Build()
        {
            var lookup = mappings.SelectMany(kvp =>
             {
                 return kvp.Value.Select(v => new KeyValuePair<PathString, string>(kvp.Key, v));
             }).ToLookup(kvp => kvp.Key, kvp => kvp.Value);

            return new AccessOptions(lookup);
        }

        public IAccessMappingBuilder MapPath(PathString path, string ipAddress, params string[] optionalIpAddresses)
        {
            var newIpAddresses = new List<string>();
            newIpAddresses.Add(ipAddress);
            newIpAddresses.AddRange(optionalIpAddresses);

            mappings[path] = newIpAddresses;

            return this;
        }
    }
}
