using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AspNetCore.Access
{
    public class AccessOptions
    {
        public AccessOptions(ILookup<PathString, string> mappings)
        {
            Mappings = mappings;
        }

        public ILookup<PathString, string> Mappings { get; }
    }
}
