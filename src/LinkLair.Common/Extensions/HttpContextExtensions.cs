using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace LinkLair.Common.Extensions;

public static class HttpContextExtensions
{
    public static string GetIp(this HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        var remoteIpAddress = httpContext.Connection?.RemoteIpAddress?.MapToIPv4();

        var headers = httpContext.Request?.Headers;
        string ip = null;

        if (headers != null)
        {
            ip = GetHeaderValue(headers, "X-Forwarded-For");
        }

        if (string.IsNullOrWhiteSpace(ip) && remoteIpAddress != null)
        {
            ip = remoteIpAddress.ToString();
        }

        if (string.IsNullOrWhiteSpace(ip) && headers != null)
        {
            ip = GetHeaderValue(headers, "REMOTE_ADDR");
        }

        return ip;
    }

    private static string GetHeaderValue(IHeaderDictionary headers, string headerName)
    {
        var values = headers[headerName];
        return values == StringValues.Empty ? null : values.First().Split(',').FirstOrDefault();
    }
}
