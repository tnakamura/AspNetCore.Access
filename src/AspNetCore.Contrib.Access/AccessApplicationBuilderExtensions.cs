using System;
using AspNetCore.Contrib.Access;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for adding the <see cref="AccessMiddleware"/>
    /// middleware to an <see cref="IApplicationBuilder"/>.
    /// </summary>
    public static class AccessApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a <see cref="AccessMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>
        /// with the <see cref="AccessOptions"/> built from configured <see cref="IAccessMappingBuilder"/>.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="IApplicationBuilder"/> to add the middleware to.
        /// </param>
        /// <param name="action">
        /// An <see cref="Action{IAccessMappingBuilder}"/> to configure
        /// the provided <see cref="IAccessMappingBuilder"/>.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed</returns>
        public static IApplicationBuilder UseAccess(this IApplicationBuilder builder, Action<IAccessMappingBuilder> action)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var mappingBuilder = new AccessMappingBuilder();
            action(mappingBuilder);
            var options = mappingBuilder.Build();
            return builder.UseMiddleware<AccessMiddleware>(options);
        }
    }
}
