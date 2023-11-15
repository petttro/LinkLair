using LinkLair.Security.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace LinkLair.Security.Authorization.Handlers;

public class HasIdentityAuthorizationHandler<T> : AuthorizationHandler<T>
    where T : IIdentityRequirement
{
    private readonly ILogger _logger;

    public HasIdentityAuthorizationHandler(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<HasIdentityAuthorizationHandler<T>>();
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
    {
        _logger.LogDebug($"Requirement={requirement.GetType().FullName}, ExpectedIdentities={GetExpectedIdentitiesNames(requirement)}. " +
                         $"Making sure user has required identity");

        if (context.User.Identities.Any(x => requirement.AllowedIdentities.Contains(x.GetType())))
        {
            context.Succeed(requirement);
            _logger.LogDebug("Found required identity");
        }
        else
        {
            _logger.LogDebug($"Unable to find ExpectedIdentities={string.Join(", ", requirement.AllowedIdentities.Select(x => x.FullName))}");
        }

        return Task.CompletedTask;
    }

    private static string GetExpectedIdentitiesNames(T requirement)
    {
        return string.Join(", ", requirement.AllowedIdentities.Select(x => x.FullName));
    }
}
