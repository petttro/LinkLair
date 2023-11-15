using LinkLair.Security.Identities;

namespace LinkLair.Security.Authorization.Requirements;

public class ClientRequirement : IIdentityRequirement
{
    public IEnumerable<Type> AllowedIdentities { get; } = new List<Type> { typeof(ClientIdentity) };
}
