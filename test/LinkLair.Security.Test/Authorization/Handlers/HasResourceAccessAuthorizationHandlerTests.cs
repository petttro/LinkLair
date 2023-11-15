using System.Security.Claims;
using LinkLair.Security.Authorization.Handlers;
using LinkLair.Security.Authorization.Requirements;
using LinkLair.Security.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Xunit;

namespace LinkLair.Security.Test.Authorization.Handlers;

public class HasResourceAccessAuthorizationHandlerTests
{
    public static IEnumerable<object[]> TestClaimInput =
        new List<object[]> { new object[] { new[] { new Claim(Resource, ClaimRead) } } };

    private const string Resource = "SomeResource";
    private const string ClaimRead = "Read";
    private readonly HasResourceAccessAuthorizationHandler _handler;

    public HasResourceAccessAuthorizationHandlerTests()
    {
        _handler = new HasResourceAccessAuthorizationHandler(new LoggerFactory());
    }

    [Theory]
    [MemberData(nameof(TestClaimInput))]
    public async Task HasResourceAccessAuthorizationHandler_ResourceAndClaimFound_Success(Claim[] claims)
    {
        // Arrange
        var requirements = new List<IAuthorizationRequirement>
        {
            new ResourceRequirement(Resource, new List<string> { ClaimRead })
        };

        var claimsPrincipal = new ClaimsPrincipal(new ClientIdentity(claims));
        var context = new AuthorizationHandlerContext(requirements, claimsPrincipal, null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.True(context.HasSucceeded);
    }

    [Theory]
    [MemberData(nameof(TestClaimInput))]
    public async Task HasResourceAccessAuthorizationHandler_ResourceAndClaimNotFound_Failure(Claim[] claims)
    {
        // Arrange
        var requirements = new List<IAuthorizationRequirement>
        {
            new ResourceRequirement("Some Other Resource", new List<string> { ClaimRead })
        };

        var claimsPrincipal = new ClaimsPrincipal(new ClientIdentity(claims));
        var context = new AuthorizationHandlerContext(requirements, claimsPrincipal, null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }
}
