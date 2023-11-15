using LinkLair.Security.Authorization.Policies;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace LinkLair.Security.Test.Authorization.Policies;

public class AllowClientsWithLinkLairReadAccessPolicyTests
{
    [Fact]
    public void AllowClientsWithLinkLairReadAccessPolicy_Statics_Success()
    {
        Assert.Equal("Allow:Clients:LinkLair:Read", AllowClientsWithLinkLairReadAccessPolicy.Name);
        Assert.NotNull(AllowClientsWithLinkLairReadAccessPolicy.Requirements);
        Assert.IsType<IAuthorizationRequirement[]>(AllowClientsWithLinkLairReadAccessPolicy.Requirements);
    }
}
