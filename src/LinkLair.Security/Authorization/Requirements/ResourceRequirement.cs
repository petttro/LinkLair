using Microsoft.AspNetCore.Authorization;

namespace LinkLair.Security.Authorization.Requirements;

public class ResourceRequirement : IAuthorizationRequirement
{
    public ResourceRequirement(string resourceType, IEnumerable<string> allowedOperations)
    {
        ResourceType = resourceType ?? throw new ArgumentNullException(nameof(resourceType));
        AllowedOperations = allowedOperations ?? throw new ArgumentNullException(nameof(allowedOperations));
    }

    public string ResourceType { get; }

    public IEnumerable<string> AllowedOperations { get; }
}
