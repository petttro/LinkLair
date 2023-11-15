using System.Security.Claims;
using LinkLair.Security.Authorization.Requirements;

namespace LinkLair.Security.Test;

public class PrincipalDataSource
{
    public static IEnumerable<object[]> Source => new List<object[]>
    {
        new object[] { new ClaimsPrincipal(new AuthorizedIdentity()), true },
        new object[] { new ClaimsPrincipal(new OtherAuthorizedIdentity()), true },
        new object[] { new ClaimsPrincipal(new OtherOtherAuthorizedIdentity()), true },
        new object[] { new ClaimsPrincipal(), false },
        new object[] { new ClaimsPrincipal(new UnauthorizedIdentity()), false },
        new object[] { new ClaimsPrincipal(new List<ClaimsIdentity> { new UnauthorizedIdentity(), new AuthorizedIdentity() }), true },
    };

    internal class TestClaimRequirement : IIdentityRequirement
    {
        public IEnumerable<Type> AllowedIdentities { get; } = new List<Type>
        {
            typeof(AuthorizedIdentity), typeof(OtherAuthorizedIdentity), typeof(OtherOtherAuthorizedIdentity)
        };
    }

    private class AuthorizedIdentity : ClaimsIdentity
    {
    }

    private class OtherAuthorizedIdentity : ClaimsIdentity
    {
    }

    private class OtherOtherAuthorizedIdentity : ClaimsIdentity
    {
    }

    private class UnauthorizedIdentity : ClaimsIdentity
    {
    }
}
