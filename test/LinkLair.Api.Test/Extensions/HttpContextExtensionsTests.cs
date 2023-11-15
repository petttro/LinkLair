using System;
using System.Net;
using LinkLair.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace LinkLair.Api.Test.Extensions;

public class HttpContextExtensionsTests
{
    [Fact]
    public void HttpContextExtensions_GetIp_NullParameterThrowsArgumentNullException()
    {
        HttpContext httpContext = null;

        Assert.Throws<ArgumentNullException>(() => HttpContextExtensions.GetIp(httpContext));
    }

    [Fact]
    public void HttpContextExtensions_GetIp_HeadersIsNotNull_Success()
    {
        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Add("X-Forwarded-For", new StringValues("value"));

        var content = HttpContextExtensions.GetIp(httpContext);

        Assert.NotNull(content);
        Assert.Equal("value", content);
    }

    [Fact]
    public void HttpContextExtensions_GetIp_HeadersIsNotNullAndRemoteIpAddressIsNotNull_Success()
    {
        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        httpContext.Request.Headers.Add("X-Forwarded-For", new StringValues(string.Empty));

        var content = HttpContextExtensions.GetIp(httpContext);

        Assert.NotNull(content);
        Assert.Equal("127.0.0.1", content);
    }

    [Fact]
    public void HttpContextExtensions_GetIp_HeadersIsNotNullAndRemoteAddressFromHeader_Success()
    {
        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Add("X-Forwarded-For", new StringValues(string.Empty));
        httpContext.Request.Headers.Add("REMOTE_ADDR", new StringValues("127.0.0.1"));

        var content = HttpContextExtensions.GetIp(httpContext);

        Assert.NotNull(content);
        Assert.Equal("127.0.0.1", content);
    }
}
