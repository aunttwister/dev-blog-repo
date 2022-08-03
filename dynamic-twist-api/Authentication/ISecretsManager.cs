namespace dynamic_twist_api.Authentication
{
    public interface ISecretsManager
    {
        public Task<string> GetApiKey();
    }
}
