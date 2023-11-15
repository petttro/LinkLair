using LinkLair.Security.Authorization.Policies;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace LinkLair.Security.Test.Authorization.Policies;

public class AllowClientsWithLinkLairChangeAccessPolicyTests
{
    [Fact]
    public void AllowClientsWithLinkLairChangeAccessPolicy_Statics_Success()
    {
        Assert.Equal("Allow:Clients:LinkLair:Change", AllowClientsWithLinkLairChangeAccessPolicy.Name);
        Assert.NotNull(AllowClientsWithLinkLairChangeAccessPolicy.Requirements);
        Assert.IsType<IAuthorizationRequirement[]>(AllowClientsWithLinkLairChangeAccessPolicy.Requirements);
    }
}
