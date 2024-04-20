using Microsoft.Extensions.Options;

namespace dynamic_twist_api.Application.Core.Authetication
{
    public class ApiKeyAuthenticationPostConfigureOptions : IPostConfigureOptions<ApiKeyAuthenticationOptions>
    {
        public void PostConfigure(string name, ApiKeyAuthenticationOptions options) 
        {
        }
    }
}
