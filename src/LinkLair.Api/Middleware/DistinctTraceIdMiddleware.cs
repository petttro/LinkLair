using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LinkLair.Api.Middleware;

/// <summary>
/// Override the built in context traceidentifier, which is not distinct across servers, with a value that is.
/// </summary>
public class DistinctTraceIdMiddleware
{
    private readonly RequestDelegate _next;

    public DistinctTraceIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.TraceIdentifier = Guid.NewGuid().ToString("N");
        await _next(context);
    }
}
