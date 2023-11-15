namespace LinkLair.Api.Configs;

public static class ServiceConstants
{
    // Template for Splunk Url
    public const string SplunkUrlTemplate = "https://splunkcloud.com/en-US/app/search/search?earliest={0}&latest={1}&q=search%20index%3D{2}%20Id%3D{3}%20";
}
