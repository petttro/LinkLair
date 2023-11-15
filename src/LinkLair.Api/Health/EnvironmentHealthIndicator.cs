using System;
using System.Net;
using System.Threading.Tasks;
using LinkLair.Api.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LinkLair.Api.Health;

public class EnvironmentHealthIndicator : BaseHealthIndicator
{
    private readonly ILogger _logger;
    private readonly EnvironmentConfig _envConfig;

    public EnvironmentHealthIndicator(ILogger<EnvironmentHealthIndicator> logger, IOptions<EnvironmentConfig> envOptions)
    {
        _logger = logger;
        _envConfig = envOptions.Value;
    }

    public override string Identifier => "Environment Configuration";

    public override bool Verbose => false;

    public override async Task<HealthIndicatorModel> CheckStatusAsync()
    {
        try
        {
            var label = Environment.GetEnvironmentVariable("LABEL");

            if (string.IsNullOrWhiteSpace(label))
            {
                label = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            }

            StartTimeMeasurement();

            var model = new HealthIndicatorModel
            {
                Name = Identifier,
                Details = label
            };

            model.AddResult(GetBuildTypeModel());
            model.AddResult(GetDotnetCoreModel());
            model.AddResult(GetBuildInfoModel());

            StopTimeMeasurement();

            return await Task.FromResult(model);
        }
        catch (Exception ex)
        {
            StopTimeMeasurement();

            _logger.LogError(ex, "Unhandled Exception during HealthIndicator Check");

            return await Task.FromResult(new HealthIndicatorModel
            {
                Status = HttpStatusCode.InternalServerError,
                Details = "Unhandled Exception during EnvironmentHealthIndicator check: " + ex,
                Name = Identifier,
                ExecutionTimeInMilliseconds = ElapsedTimeImMilliseconds
            });
        }
    }

    private HealthIndicatorModel GetDotnetCoreModel()
    {
        var runtimePath = System.IO.Path.GetDirectoryName(typeof(object).Assembly.Location);
        string version;

        try
        {
            runtimePath = runtimePath.Replace("\\", "/");
            version = runtimePath.Substring(runtimePath.LastIndexOf('/') + 1);
        }
        catch (Exception)
        {
            // Just return the runtimePath if we have trouble parsing.
            version = runtimePath ?? "Could not be determined";
        }

        var model = new HealthIndicatorModel(".Net Core Version", HttpStatusCode.OK, version);

        model.ExecutionTimeInMilliseconds = ElapsedTimeImMilliseconds;

        return model;
    }

    private HealthIndicatorModel GetBuildInfoModel()
    {
        var version = _envConfig.BuildVersion;

        version = version == "BAMBOO_WILL_REPLACE_THIS_VALUE" ? "Local Development" : version.Replace("~", "__");

        var model = new HealthIndicatorModel("Build Version", HttpStatusCode.OK, version);
        model.ExecutionTimeInMilliseconds = ElapsedTimeImMilliseconds;

        return model;
    }

    private HealthIndicatorModel GetBuildTypeModel()
    {
        var model = new HealthIndicatorModel("Build Type", HttpStatusCode.ServiceUnavailable, "UNKNOWN");

#if DEBUG
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        model = new HealthIndicatorModel("Build Type", environment == "Development" ? HttpStatusCode.OK : HttpStatusCode.InternalServerError, "DEBUG");
#endif

#if RELEASE
        model = new HealthIndicatorModel("Build Type", HttpStatusCode.OK, "RELEASE");
#endif

        model.ExecutionTimeInMilliseconds = ElapsedTimeImMilliseconds;

        return model;
    }
}
