using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Access
{
    class AccessMappingBuilder : IAccessMappingBuilder
    {
        readonly Dictionary<PathString, List<IPAddress>> _mappings =
            new Dictionary<PathString, List<IPAddress>>();

        public AccessOptions Build()
        {
            var lookup = _mappings.SelectMany(kvp =>
             {
                 return kvp.Value.Select(v => new KeyValuePair<PathString, IPAddress>(kvp.Key, v));
             }).ToLookup(kvp => kvp.Key, kvp => kvp.Value);

            return new AccessOptions(lookup);
        }

        public IAccessMappingBuilder MapPath(PathString path, string ipAddress, params string[] optionalIpAddresses)
        {
            var newIpAddresses = new List<IPAddress>();

            newIpAddresses.Add(Parse(ipAddress, nameof(ipAddress)));

            foreach (var optionalIpAddress in optionalIpAddresses)
            {
                newIpAddresses.Add(Parse(optionalIpAddress, nameof(optionalIpAddresses)));
            }

            _mappings[path] = newIpAddresses;

            return this;
        }

        IPAddress Parse(string ipString, string paramName)
        {
            if (IPAddress.TryParse(ipString, out var address))
            {
                return address;
            }
            else
            {
                throw new ArgumentException(
                    $"'{ipString}' is not IP address or mask.",
                    paramName);
            }
        }
    }
}
