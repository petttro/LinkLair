using LinkLair.Security.Authorization.Requirements;
using Microsoft.Extensions.Logging;

namespace LinkLair.Security.Authorization.Handlers;

public class HasClientIdentityAuthorizationHandler :
    HasIdentityAuthorizationHandler<ClientRequirement>
{
    public HasClientIdentityAuthorizationHandler(ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
    }
}
