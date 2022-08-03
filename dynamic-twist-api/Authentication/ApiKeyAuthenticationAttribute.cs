using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dynamic_twist_api.Authentication
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ApiKeyAuthenticationAttribute : Attribute, IAsyncActionFilter
    {
        public const string ApiKey = "ApiKeyName";
        private ISecretsManager _secretsManager;
        public ApiKeyAuthenticationAttribute(ISecretsManager secretsManager)
        {
            _secretsManager = secretsManager;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var potentialApiKey))
            {
                context.Result = new UnauthorizedResult();
            }

            if (potentialApiKey == _secretsManager.GetApiKey())
            {
                await next();
            }
        }
    }
}
