using dynamic_twist_api.Application.Infrastructure.SecretsManager;

namespace dynamic_twist_api.Application.Core.Authetication
{
    public class ApiKeyAuthenticationService : IApiKeyAuthenticationService
    {
        public readonly IConfiguration _configuration;
        public ApiKeyAuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> IsValidAsync(string apiKey)
        {
            var secrets = _configuration.GetSection(SecretsOptions.Position).Get<SecretsOptions>();

            return apiKey == await SecretsManager.GetApiKey(secrets);
        }
    }
}
