namespace LinkLair.Api.Configs;

public class SplunkConfig
{
    // do not change. Environment variable SPLUNKLABEL is set in run_service.sh during docker image creation
    public string SplunkLabel { get; set; }

    // do not change. Environment variable SYSTEM_ENV is set in run_service.sh during docker image creation
    public string System_Env { get; set; }

    public string Index => $"{SplunkLabel}_{System_Env}";

    public bool IsValidIndex => !string.IsNullOrWhiteSpace(SplunkLabel) && !string.IsNullOrWhiteSpace(System_Env);
}
