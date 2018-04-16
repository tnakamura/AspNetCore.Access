using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AspNetCore.Access
{
    /// <summary>
    /// Represents a middleware for limiting access based on IP address
    /// </summary>
    public class AccessMiddleware
    {
        readonly RequestDelegate _next;

        readonly AccessOptions _options;

        /// <summary>
        /// Create a new instance of <see cref="AccessMiddleware"/>
        /// </summary>
        /// <param name="next">
        /// The delegate representing the next middleware in the request pipeline.
        /// </param>
        /// <param name="options">
        /// The middleware options.
        /// </param>
        public AccessMiddleware(RequestDelegate next, AccessOptions options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Executes the middleware.
        /// </summary>
        /// <param name="context">
        /// The <see cref="HttpContext"/> for the current request.
        /// </param>
        /// <returns>
        /// A task that represents the execution of this middleware.
        /// </returns>
        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var ipmasks = GetIPMasksForPath(context.Request.Path);
            if (IsIPAddressAuthorized(context, ipmasks))
            {
                await _next(context);
            }
            else
            {
                Forbidden(context);
            }
        }

        void Forbidden(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }

        IEnumerable<IPAddress> GetIPMasksForPath(PathString path)
        {
            return _options.Mappings
                .Where(m => path.StartsWithSegments(m.Key))
                .SelectMany(m => m);
        }

        bool IsIPAddressAuthorized(HttpContext context, IEnumerable<IPAddress> ipmasks)
        {
            var isAuthorized = true;
            var remoteIp = context.Connection.RemoteIpAddress;
            foreach (var ipmask in ipmasks)
            {
                if (ipmask.Equals(remoteIp))
                {
                    isAuthorized = true;
                    break;
                }
                else
                {
                    isAuthorized = false;
                }
            }
            return isAuthorized;
        }
    }
}
