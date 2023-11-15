using System.Security.Claims;
using LinkLair.Security.Identities;

namespace LinkLair.Security.Test;

public class IdentityMock
{
    public static ClientIdentity DefaultClientIdentity => new IdentityMock().CreateClientIdentity();

    public string ApplicationName { get; set; } = "Application";

    public static List<Claim> CreateClientIdentityClaims(string applicationName)
    {
        return new List<Claim>
        {
            new (AuthCraftClaimTypes.Client.ApplicationName, applicationName)
        };
    }

    public static ClientIdentity CreateClientIdentity(string applicationName)
    {
        return new ClientIdentity(CreateClientIdentityClaims(applicationName));
    }

    public ClientIdentity CreateClientIdentity()
    {
        var claims = CreateClientIdentityClaims(ApplicationName);
        return new ClientIdentity(claims);
    }
}
