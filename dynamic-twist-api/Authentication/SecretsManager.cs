using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Options;

namespace dynamic_twist_api.Authentication
{
    public class SecretsManager : ISecretsManager
    {
        private readonly SecretsOptions _secretsOptions;
        public SecretsManager(IOptions<SecretsOptions> secretsOptions)
        {
            _secretsOptions = secretsOptions.Value;
        }
        public async Task<string> GetApiKey()
        {
            SecretManagerServiceClient client = SecretManagerServiceClient.Create();

            var secret = await client.AccessSecretVersionAsync(_secretsOptions.ApiKeyName);
            return secret.Payload.ToString();
        }
    }
}
