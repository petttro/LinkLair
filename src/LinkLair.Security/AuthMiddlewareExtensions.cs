using System.Security.Cryptography;
using LinkLair.Security.Authentication;
using LinkLair.Security.Authorization.Handlers;
using LinkLair.Security.Authorization.Policies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LinkLair.Security;

public static class AuthMiddlewareExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthConfig>(configuration.GetSection(AuthConfig.SectionName));
        var authConfig = configuration.GetSection(AuthConfig.SectionName).Get<AuthConfig>();

        services.AddAuthentication(options =>
        {
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var publicRsa = RSA.Create();
            publicRsa.ImportFromPem(authConfig.JwtRsaPublicKey);

            var publicSecurityKey = new RsaSecurityKey(publicRsa);

            options.Audience = authConfig.Audience;
            options.IncludeErrorDetails = true;
            options.ClaimsIssuer = "claims issuer";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = publicSecurityKey,
                ValidateIssuer = true,
                ValidIssuer = authConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = authConfig.Audience,
                ValidateLifetime = true,

                // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time when validating
                // the lifetime. When creating the tokens locally and validating them on the same machines which should have
                // synchronised time, this can be set to zero. Where external tokens are used, some leeway here could be useful.
                ClockSkew = TimeSpan.FromMilliseconds(authConfig.SkewTimeInMilliseconds),
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = JwtSecurityTokenHandlerEvents.OnTokenValidated
            };
        });

        return services;
    }

    public static FilterCollection AddRequireAuthenticatedUserFilter(this FilterCollection collection)
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .Build();
        collection.Add(new AuthorizeFilter(policy));
        return collection;
    }

    public static IServiceCollection AddJwtAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, HasClientIdentityAuthorizationHandler>();
        services.AddSingleton<IAuthorizationHandler, HasResourceAccessAuthorizationHandler>();

        return services.AddAuthorization(o =>
        {
            o.AddPolicy(AllowClientsPolicy.Name, p =>
            {
                foreach (var req in AllowClientsPolicy.Requirements)
                {
                    p.AddRequirements(req);
                }

                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });

            o.AddPolicy(AllowClientsWithLinkLairReadAccessPolicy.Name, p =>
            {
                foreach (var req in AllowClientsWithLinkLairReadAccessPolicy.Requirements)
                {
                    p.AddRequirements(req);
                }

                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });

            o.AddPolicy(AllowClientsWithLinkLairChangeAccessPolicy.Name, p =>
            {
                foreach (var req in AllowClientsWithLinkLairChangeAccessPolicy.Requirements)
                {
                    p.AddRequirements(req);
                }

                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });

            o.AddPolicy(AllowClientsWithLinkLairDeleteAccessPolicy.Name, p =>
            {
                foreach (var req in AllowClientsWithLinkLairDeleteAccessPolicy.Requirements)
                {
                    p.AddRequirements(req);
                }

                p.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            });
        });
    }
}
