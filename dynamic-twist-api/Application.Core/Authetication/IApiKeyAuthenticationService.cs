namespace dynamic_twist_api.Application.Core.Authetication
{
    public interface IApiKeyAuthenticationService
    {
        Task<bool> IsValidAsync(string apiKey);
    }
}
