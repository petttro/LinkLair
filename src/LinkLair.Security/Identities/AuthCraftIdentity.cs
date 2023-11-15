using System.Security.Claims;

namespace LinkLair.Security.Identities;

public class AuthCraftIdentity : ClaimsIdentity
{
    public AuthCraftIdentity(ClaimsIdentity claimsIdentity, string type)
        : base(claimsIdentity)
    {
        AddIdentityTypeClaimIfNotExists(type);
    }

    public AuthCraftIdentity(IEnumerable<Claim> claims, string type)
        : base(claims)
    {
        AddIdentityTypeClaimIfNotExists(type);
    }

    public AuthCraftIdentity(IEnumerable<Claim> claims, string authenticationType, string type)
        : base(claims, authenticationType)
    {
        AddIdentityTypeClaimIfNotExists(type);
    }

    public string GetClaim(string claimType)
    {
        return FindFirst(x => x.Type == claimType)?.Value;
    }

    private void AddIdentityTypeClaimIfNotExists(string type)
    {
        if (!HasClaim(x => x.Type == AuthCraftClaimTypes.Type))
        {
            AddClaim(new Claim(AuthCraftClaimTypes.Type, type));
        }
    }
}
