using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace LinkLair.Api;

public static class AddCustomizedMvcExtensions
{
    public static IServiceCollection AddCustomizedMvc(this IServiceCollection services, string corsPolicyName)
    {
        // Add service and create Policy with options
        // CORS must be added before MVC or it will never be hit
        services.AddCors(options =>
        {
            options.AddPolicy(
                corsPolicyName,
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        services
            .AddMvc(options =>
                options.CacheProfiles.Add(
                    "Never",
                    new CacheProfile
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    }))
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });

        services.AddMvcCore().AddApiExplorer();

        return services;
    }
}
