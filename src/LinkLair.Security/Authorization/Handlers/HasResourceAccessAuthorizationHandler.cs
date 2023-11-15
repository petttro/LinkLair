using LinkLair.Common.Extensions;
using LinkLair.Security.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace LinkLair.Security.Authorization.Handlers;

public class HasResourceAccessAuthorizationHandler : AuthorizationHandler<ResourceRequirement>
{
    private readonly ILogger _logger;

    public HasResourceAccessAuthorizationHandler(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<HasResourceAccessAuthorizationHandler>();
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceRequirement requirement)
    {
        _logger.LogDebug($"Requirement={requirement.GetType().FullName} Resource={requirement.ResourceType} " +
                         $"Operations={requirement.AllowedOperations.ToJoinedString()}. " +
                         $"Checking if requesting client has the specified permission for a given configuration");

        if (context.User == null)
        {
            _logger.LogWarning($"Unable to extract any claims from the request");
            return Task.CompletedTask;
        }

        var claims = context.User.FindAll(c => string.Compare(c.Type, requirement.ResourceType) == 0);
        var hasRequiredClaims =
            requirement.AllowedOperations.All(operation =>
                claims.Any(claim => string.Compare(claim.Value, operation, StringComparison.OrdinalIgnoreCase) == 0));

        if (hasRequiredClaims)
        {
            _logger.LogDebug($"RequiredResource={requirement.ResourceType} and Operations={requirement.AllowedOperations.ToJoinedString()} " +
                             $"found in SuppliedClaims=[{claims.ToJoinedString()}]. Authorized access");
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogWarning($"RequiredResource={requirement.ResourceType} and Operations={requirement.AllowedOperations.ToJoinedString()} " +
                               $"not found in SuppliedClaims=[{claims.ToJoinedString()}]. Unauthorized access");
        }

        return Task.CompletedTask;
    }
}
