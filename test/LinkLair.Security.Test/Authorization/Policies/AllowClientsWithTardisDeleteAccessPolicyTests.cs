using LinkLair.Security.Authorization.Policies;
using Microsoft.AspNetCore.Authorization;
using Xunit;

namespace LinkLair.Security.Test.Authorization.Policies;

public class AllowClientsWithLinkLairDeleteAccessPolicyTests
{
    [Fact]
    public void AllowClientsWithLinkLairDeleteAccessPolicy_Statics_Success()
    {
        Assert.Equal("Allow:Clients:LinkLair:Delete", AllowClientsWithLinkLairDeleteAccessPolicy.Name);
        Assert.NotNull(AllowClientsWithLinkLairDeleteAccessPolicy.Requirements);
        Assert.IsType<IAuthorizationRequirement[]>(AllowClientsWithLinkLairDeleteAccessPolicy.Requirements);
    }
}
