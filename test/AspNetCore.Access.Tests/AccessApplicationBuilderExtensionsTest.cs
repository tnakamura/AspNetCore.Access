using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AspNetCore.Access.Tests
{
    public class AccessApplicationBuilderExtensionsTest
    {
        [Fact]
        public async Task Test()
        {
            var app = new ApplicationBuilder(Mock.Of<IServiceProvider>());
            app.UseAccess(mappings =>
            {
                mappings.MapPath("/swagger", "127.0.0.1");
            });
            var appFunc = app.Build();

            var context = new DefaultHttpContext();
            context.Request.Path = "/swagger";
            context.Connection.RemoteIpAddress = IPAddress.Parse("192.1.1.1");
            await appFunc(context);

            Assert.Equal((int)HttpStatusCode.Forbidden, context.Response.StatusCode);
        }

        [Fact]
        public async Task Build_add_loopback_address_if_empty()
        {
            var app = new ApplicationBuilder(Mock.Of<IServiceProvider>());
            app.UseAccess(mappings =>
            {
            });
            var appFunc = app.Build();

            var context = new DefaultHttpContext();
            context.Request.Path = "/";
            context.Connection.RemoteIpAddress = IPAddress.Parse("192.1.1.1");
            await appFunc(context);
            Assert.Equal((int)HttpStatusCode.Forbidden, context.Response.StatusCode);
        }
    }
}
