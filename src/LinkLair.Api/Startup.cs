using System;
using LinkLair.Api.Configs;
using LinkLair.Api.Middleware;
using LinkLair.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkLair.Api;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly string CorsPolicyName = "CorsPolicy";

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to add services to the container.
    /// </summary>
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
        services.AddMvc(options => options.EnableEndpointRouting = false);
        services.AddSingleton(_configuration);

        // Get configuration values from Environment Variables and/or from launchSettings.json
        services.Configure<EnvironmentConfig>(_configuration.GetSection("Environment"));
        services.Configure<SplunkConfig>(_configuration);

        services.AddOptions(); // Setup options with DI
        services.AddLogging();
        services.AddHttpContextAccessor();
        services.AddCustomizedMvc(CorsPolicyName);
        services.AddSwaggerGen();
        services.AddCustomServices();

        services.AddJwtAuthentication(_configuration);
        services.AddJwtAuthorization();

        return services.BuildServiceProvider();
    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">App builder.</param>
    public void Configure(IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseDefaultFiles();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "LinkLair API V1");
            c.RoutePrefix = "help";
        });

        app.UseMiddleware<DistinctTraceIdMiddleware>();
        app.UseMiddleware<CustomHeadersMiddleware>();

        app.UseMiddleware<LogResponseMiddleware>();
        app.UseMiddleware<LogRequestMiddleware>();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();

        // Apply CORS policy globally.  It can also be applied specifically to controllers if global policy is not desirable
        // NOTE: This must be declared before app.UseMvc();
        app.UseCors(CorsPolicyName);
        app.UseMvc();
    }
}
