namespace dynamic_twist_api.Application.Infrastructure.SecretsManager
{
    public class SecretsOptions
    {
        public const string Position = "Secrets";
        public string ApiKeyName { get; set; }
        public string CredentialsPath { get; set; }
        public string ProjectName { get; set; }
        public string ProjectVersion { get; set; }
    }
}
