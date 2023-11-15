using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LinkLair.Security.Test;

public class AuthMiddlewareExtensionsTests
{
    [Fact]
    public void AuthMiddlewareExtensions_UseJwtAuth_NullConfigurationThrowsNullReferenceException()
    {
        IServiceCollection serviceCollection = null;
        IConfiguration configuration = null;

        Assert.Throws<NullReferenceException>(() => serviceCollection.AddJwtAuthentication(configuration));
    }

    [Fact]
    public void AuthMiddlewareExtensions_UseJwtAuth_MissedSecurityKeyInConfigurationThrowsArgumentNullException()
    {
        IConfiguration configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        IServiceCollection serviceCollection = null;

        Assert.Throws<ArgumentNullException>(() => serviceCollection.AddJwtAuthentication(configuration));
    }

    [Fact]
    public void AuthMiddlewareExtensions_UseJwtAuth_NullAppThrowsArgumentNullException()
    {
        IConfiguration configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        configuration["Auth:SecurityKey"] = "SecurityKey";
        IServiceCollection serviceCollection = null;

        Assert.Throws<ArgumentNullException>(() => serviceCollection.AddJwtAuthentication(configuration));
    }

    [Fact]
    public void AuthMiddlewareExtensions_AddRequireAuthenticatedUserFilter_NullParameterThrowsNullReferenceException()
    {
        FilterCollection collection = null;

        Assert.Throws<NullReferenceException>(() => collection.AddRequireAuthenticatedUserFilter());
    }

    [Fact]
    public void AuthMiddlewareExtensions_AddRequireAuthenticatedUserFilter_Success()
    {
        FilterCollection collection = new FilterCollection();

        Assert.Empty(collection);

        var returnedApp = collection.AddRequireAuthenticatedUserFilter();

        Assert.Single(returnedApp);
        var policy = returnedApp[0];
        Assert.NotNull(policy);
        Assert.IsType<AuthorizeFilter>(policy);
    }

    [Fact]
    public void AuthMiddlewareExtensions_AddRequireAuthentication_Success()
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        Assert.Empty(serviceCollection);

        var returnedApp = serviceCollection.AddJwtAuthorization();

        Assert.NotEmpty(returnedApp);
    }
}
