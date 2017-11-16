using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Access.Tests
{
    public class AccessMiddlewareTest
    {
        [Fact]
        public async Task Invoke_CallNext_WhenMatched()
        {
            var builder = new AccessMappingBuilder();
            builder.MapPath("/swagger", "127.0.0.1");
            var options = builder.Build();
            var middleware = new AccessMiddleware(context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                return Task.CompletedTask;
            }, options);

            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            httpContext.Request.Path = "/swagger";
            await middleware.Invoke(httpContext);

            Assert.Equal((int)HttpStatusCode.OK, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_CallNext_WhenOtherPath()
        {
            var builder = new AccessMappingBuilder();
            builder.MapPath("/swagger", "127.0.0.1");
            var options = builder.Build();
            var middleware = new AccessMiddleware(context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                return Task.CompletedTask;
            }, options);

            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("192.1.1.1");
            httpContext.Request.Path = "/api";
            await middleware.Invoke(httpContext);

            Assert.Equal((int)HttpStatusCode.OK, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_BlockAccess_WhenNotAllowed()
        {
            var builder = new AccessMappingBuilder();
            builder.MapPath("/swagger", "192.1.1.1");
            var options = builder.Build();
            var middleware = new AccessMiddleware(context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                return Task.CompletedTask;
            }, options);

            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            httpContext.Request.Path = "/swagger";
            await middleware.Invoke(httpContext);

            Assert.Equal((int)HttpStatusCode.Forbidden, httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_CallNext_AllowedOnly()
        {
            var builder = new AccessMappingBuilder();
            builder.MapPath("/swagger", "127.0.0.1", "192.1.1.1");
            var options = builder.Build();
            var middleware = new AccessMiddleware(context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                return Task.CompletedTask;
            }, options);

            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            httpContext.Request.Path = "/swagger";
            await middleware.Invoke(httpContext);
            Assert.Equal((int)HttpStatusCode.OK, httpContext.Response.StatusCode);

            httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("192.1.1.1");
            httpContext.Request.Path = "/swagger";
            await middleware.Invoke(httpContext);
            Assert.Equal((int)HttpStatusCode.OK, httpContext.Response.StatusCode);

            httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("130.1.1.1");
            httpContext.Request.Path = "/swagger";
            await middleware.Invoke(httpContext);
            Assert.Equal((int)HttpStatusCode.Forbidden, httpContext.Response.StatusCode);
        }
    }
}
