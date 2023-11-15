using System.Security.Claims;
using LinkLair.Common.Test;
using LinkLair.Security.Authentication;
using LinkLair.Security.Identities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Xunit;

namespace LinkLair.Security.Test.Authentication;

public class JwtSecurityTokenHandlerEventsTests
{
    private readonly TokenValidatedContext _tokenContext;

    public JwtSecurityTokenHandlerEventsTests()
    {
        var httpContext = new TestHttpContext();
        var bearerOptions = new JwtBearerOptions();
        var scheme = new AuthenticationScheme(
            JwtBearerDefaults.AuthenticationScheme,
            JwtBearerDefaults.AuthenticationScheme,
            typeof(AuthenticationHandler<AuthenticationSchemeOptions>));
        _tokenContext = new TokenValidatedContext(httpContext, scheme, bearerOptions);
    }

    [Fact]
    public async Task JwtSecurityTokenHandlerEvents_OnTokenValidated_NoClaims_Success()
    {
        _tokenContext.Principal = new ClaimsPrincipal(new ClaimsIdentity());

        await JwtSecurityTokenHandlerEvents.OnTokenValidated(_tokenContext);

        Assert.NotNull(_tokenContext.Principal);
    }

    [Fact]
    public async Task JwtSecurityTokenHandlerEvents_OnTokenValidated_ClientIdentity_Success()
    {
        _tokenContext.Principal = new ClaimsPrincipal(IdentityMock.CreateClientIdentity("accessLevel"));

        await JwtSecurityTokenHandlerEvents.OnTokenValidated(_tokenContext);

        Assert.NotNull(_tokenContext.Principal);
        Assert.Equal(_tokenContext.Principal.Claims.FirstOrDefault(x => x.Type == AuthCraftClaimTypes.Type)?.Value, ClientIdentity.Type);
    }
}
