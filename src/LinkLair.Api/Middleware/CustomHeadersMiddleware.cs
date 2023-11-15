using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LinkLair.Api.Middleware;

public class CustomHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public CustomHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.Headers["x-linklair-correlationid"] = context.TraceIdentifier;
        await _next(context);
    }
}
