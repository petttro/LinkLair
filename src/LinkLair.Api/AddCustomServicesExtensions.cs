using LinkLair.Api.Health;
using LinkLair.Common.DynamoDb;
using LinkLair.Data.Repositories;
using LinkLair.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LinkLair.Api;

public static class AddCustomServicesExtensions
{
    /// <summary>
    /// Configure custom self written services.
    /// </summary>
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IHealthIndicator, EnvironmentHealthIndicator>()
            .AddTransient<IClientResourceService, ClientResourceService>()
            .AddTransient<IClientResourceRepository, ClientResourceRepository>()
            .AddSingleton<IDynamoDbConnection, DynamoDbConnection>()
            .AddSingleton<IDynamoDbContext, DynamoDbContext>();

        return services;
    }
}
