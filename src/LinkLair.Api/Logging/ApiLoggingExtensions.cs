using System.Linq;
using LinkLair.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace LinkLair.Api.Logging;

public static class ApiLoggingExtensions
{
    private const int MaxHeaderLength = 10000;

    public static void LogRequest(this ILogger logger, HttpContext context, string requestBody)
    {
        var ipAddress = context.GetIp();

        LogMessage(
            logger,
            "ApiRequest",
            ipAddress,
            context,
            context.Request.Headers,
            $"'{requestBody}'",
            0);
    }

    public static void LogResponse(this ILogger logger, HttpContext context, string responseBody, long durationMilliseconds)
    {
        var ipAddress = context.GetIp();

        LogMessage(
            logger,
            "ApiResponse",
            ipAddress,
            context,
            context.Response.Headers,
            responseBody,
            context.Response.StatusCode,
            durationMilliseconds);
    }

    private static void LogMessage(
        ILogger logger,
        string messageType,
        string ipAddress,
        HttpContext context,
        IHeaderDictionary headers,
        string body,
        int statusCode,
        long durationMillis = long.MinValue)
    {
        var headersString = GetHeaderTraceString(headers);
        var durationMillisString = durationMillis == long.MinValue ? string.Empty : $"DurationMillis={durationMillis}, ";

        logger.LogInformation(
            $"{messageType}, Verb={context.Request.Method}, Url={context.Request.GetDisplayUrl()}, Status={statusCode}, " +
            $"Ip={ipAddress}, {durationMillisString}Headers=[{headersString}], Body={body}");
    }

    private static string GetHeaderTraceString(IHeaderDictionary headers)
    {
        if (headers == null || !headers.Any())
        {
            return null;
        }

        var joinedHeaders = string.Join(",", headers.Select(header => $"{header.Key}='{string.Join(",", header.Value.Any() ? header.Value.ToString() : string.Empty)}'"));

        // Limit the string to 1000 symbols
        return joinedHeaders.Length <= MaxHeaderLength
            ? joinedHeaders
            : joinedHeaders.Substring(0, MaxHeaderLength) + " ***** LIMITED TO " + MaxHeaderLength + " BYTES *****";
    }
}
