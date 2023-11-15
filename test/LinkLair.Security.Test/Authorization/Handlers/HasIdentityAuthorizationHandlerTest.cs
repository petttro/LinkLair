using System.Security.Claims;
using LinkLair.Security.Authorization.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace LinkLair.Security.Test.Authorization.Handlers;

public class HasIdentityAuthorizationHandlerTest
{
    private readonly HasIdentityAuthorizationHandler<PrincipalDataSource.TestClaimRequirement> _handler;

    public HasIdentityAuthorizationHandlerTest()
    {
        var loggerFactoryMock = new NullLoggerFactory();
        _handler = new HasIdentityAuthorizationHandler<PrincipalDataSource.TestClaimRequirement>(loggerFactoryMock);
    }

    [Theory]
    [MemberData(nameof(PrincipalDataSource.Source), MemberType = typeof(PrincipalDataSource))]
    public async Task HasIdentityAuthorizationHandlerBaseTest(ClaimsPrincipal claimsPrincipal, bool expectedResult)
    {
        // Arrange
        var requirements = new List<IAuthorizationRequirement> { new PrincipalDataSource.TestClaimRequirement() };
        var context = new AuthorizationHandlerContext(requirements, claimsPrincipal, null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.Equal(expectedResult, context.HasSucceeded);
    }
}
