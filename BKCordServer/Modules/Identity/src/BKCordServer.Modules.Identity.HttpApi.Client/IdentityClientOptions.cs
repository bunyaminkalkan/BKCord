namespace BKCordServer.Modules.Identity.HttpApi.Client;

public class IdentityClientOptions
{
    public const string SectionName = "IdentityClient";

    public string BaseUrl { get; set; } = "https://localhost:5001";
    public string ApiPath { get; set; } = "identity";
    public int TimeoutSeconds { get; set; } = 30;
    public bool EnableRetry { get; set; } = true;
    public int RetryCount { get; set; } = 3;
    public bool EnableCircuitBreaker { get; set; } = true;
}
