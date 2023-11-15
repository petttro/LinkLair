using Microsoft.AspNetCore.Authorization;

namespace LinkLair.Security.Authorization.Requirements;

public interface IIdentityRequirement : IAuthorizationRequirement
{
    IEnumerable<Type> AllowedIdentities { get; }
}
