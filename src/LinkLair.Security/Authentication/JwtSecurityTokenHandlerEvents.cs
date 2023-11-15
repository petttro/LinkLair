using System.Security.Claims;
using LinkLair.Security.Identities;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace LinkLair.Security.Authentication;

public static class JwtSecurityTokenHandlerEvents
{
    public static Task OnTokenValidated(TokenValidatedContext context)
    {
        var principal = context.Principal;
        var identityType = principal.Claims.FirstOrDefault(x => x.Type == AuthCraftClaimTypes.Type)?.Value;
        if (identityType == null)
        {
            return Task.CompletedTask;
        }

        var identity = principal.Identity;
        switch (identityType)
        {
            case ClientIdentity.Type:
                principal = new ClaimsPrincipal(new ClientIdentity((ClaimsIdentity)identity));
                break;
        }

        context.Principal = principal;
        context.Properties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties();

        return Task.CompletedTask;
    }
}
