namespace LinkLair.Security;

public class AuthConfig
{
    public const string SectionName = "auth";

    /// <summary>
    /// Gets or sets a string that represents a valid issuer that will be used to check against the token's issuer.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Gets or sets a string that represents a valid audience that will be used to check against the token's audience.
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time when validating
    /// the lifetime. When creating the tokens locally and validating them on the same machines which should have
    /// synchronised time, this can be set to zero. Where external tokens are used, some leeway here could be useful.
    /// </summary>
    public int SkewTimeInMilliseconds { get; set; }

    /// <summary>
    /// Gets or sets the security key  is used for signature validation.
    /// </summary>
    public string JwtRsaPublicKey { get; set; }
}
