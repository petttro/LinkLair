using LinkLair.Security.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace LinkLair.Security.Authorization.Policies;

public static class AllowClientsWithLinkLairDeleteAccessPolicy
{
    public const string Name = "Allow:Clients:LinkLair:Delete";

    public static IEnumerable<IAuthorizationRequirement> Requirements { get; } =
        new IAuthorizationRequirement[]
        {
            new ClientRequirement(),
            new ResourceRequirement(
                AuthCraftClaimTypes.Client.Resources.LinkLair,
                new List<string> { Operations.Delete.Name })
        };
}
