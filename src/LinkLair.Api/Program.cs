using System;
using System.IO;
using System.Net;
using Amazon;
using Easy.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace LinkLair.Api;

/// <summary>
/// Program entry point. Trigger change.
/// </summary>
public class Program
{
    private static readonly string HostPort = System.Environment.GetEnvironmentVariable("HOST_PORT");
    private static readonly string Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    private static IConfigurationRoot Configuration { get; } =
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", false, true)
            .AddJsonFile($"appsettings.{Environment}.json", true, true)
            .Build();

    public static void Main()
    {
        var nlogLoggingConfiguration = new NLogLoggingConfiguration(Configuration.GetSection("nlog"));
        var logger = NLogBuilder.ConfigureNLog(nlogLoggingConfiguration).GetCurrentClassLogger();

        try
        {
            var host = BuildWebHost(logger);

            var report = DiagnosticReport.Generate();
            logger.Info(report.ToString());
            host.Run();
        }
        catch (Exception ex)
        {
            logger.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            logger.Factory.Flush();
        }
    }

    private static IWebHost BuildWebHost(Logger logger) =>
        new WebHostBuilder()
            .UseKestrel(options =>
            {
                var port = int.Parse(HostPort);
                options.Listen(IPAddress.Any, port);
            })
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((context, builder) =>
            {
                // Add options from AWS SecretsManager
                logger.Info("Adding secrets from AWS SecretsManager");

                var env = System.Environment.GetEnvironmentVariable("SYSTEM_ENV");
                var region = System.Environment.GetEnvironmentVariable("AWS_REGION");
                var regionEndpoint = RegionEndpoint.GetBySystemName(region);

                builder.AddSecretsManager(
                    region: regionEndpoint,
                    configurator: options =>
                    {
                        var secretsPrefix = $"tf-pa-shared-secrets-{env}-{region}";

                        options.SecretFilter = entry => entry.Name.StartsWith(secretsPrefix);
                        options.KeyGenerator = (entry, key) => key.Substring(secretsPrefix.Length + 1);
                    });

                builder.AddEnvironmentVariables();
                builder.AddConfiguration(Configuration);
            })
            .ConfigureLogging((context, builder) =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                LogManager.Configuration = new NLogLoggingConfiguration(Configuration.GetSection("nlog"));
            })
            .UseStartup<Startup>()
            .UseNLog()
            .Build();
}
