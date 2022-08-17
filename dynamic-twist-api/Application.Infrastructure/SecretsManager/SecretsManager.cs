using Google.Cloud.SecretManager.V1;
using Microsoft.Extensions.Options;
using System;

namespace dynamic_twist_api.Application.Infrastructure.SecretsManager
{
    public static class SecretsManager
    {
        public static async Task<string> GetApiKey(SecretsOptions secretsOptions)
        {
            SecretManagerServiceClientBuilder clientBuilder = new SecretManagerServiceClientBuilder
            {
                CredentialsPath = secretsOptions.CredentialsPath
            };
            SecretManagerServiceClient client = await clientBuilder.BuildAsync();

            var secretVersionName = new SecretVersionName(
                secretsOptions.ProjectName, 
                secretsOptions.ApiKeyName, 
                secretsOptions.ProjectVersion);

            var secret = await client.AccessSecretVersionAsync(secretVersionName);

            return secret.Payload.Data.ToStringUtf8();
        }
    }
}
