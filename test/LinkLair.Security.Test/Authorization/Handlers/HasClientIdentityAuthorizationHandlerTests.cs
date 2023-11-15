using System.Security.Claims;
using LinkLair.Security.Authorization.Handlers;
using LinkLair.Security.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LinkLair.Security.Test.Authorization.Handlers;

public class HasClientIdentityAuthorizationHandlerTests
{
    [Fact] // TODO
    public async Task HasClientIdentityAuthorizationHandler_Constructor_Success()
    {
        Claim[] claims = CreateClaims();
        bool expectedResult = false;

        var handler = new HasClientIdentityAuthorizationHandler(new NullLoggerFactory());

        Assert.NotNull(handler);

        // Arrange
        var requirements = new List<IAuthorizationRequirement>
        {
            new ClientRequirement()
        };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var context = new AuthorizationHandlerContext(requirements, claimsPrincipal, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.Equal(context.HasSucceeded, expectedResult);
    }

    private static Claim[] CreateClaims()
    {
        return new[]
        {
            new Claim("type1", "value1"),
            new Claim("type3", "value3")
        };
    }
}
