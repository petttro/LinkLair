using LinkLair.Security.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace LinkLair.Security.Authorization.Policies;

public static class AllowClientsPolicy
{
    public const string Name = "Allow:Clients";

    public static IEnumerable<IAuthorizationRequirement> Requirements { get; } =
        new IAuthorizationRequirement[] { new ClientRequirement() };
}
