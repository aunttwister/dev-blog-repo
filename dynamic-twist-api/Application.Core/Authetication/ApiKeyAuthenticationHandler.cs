using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace dynamic_twist_api.Application.Core.Authetication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private const string AuthorizationHeaderName = "X-API-KEY";
        private const string ApiKeySchemeName = ApiKeyAuthenticationDefaults.AuthenticationScheme;
        private readonly IApiKeyAuthenticationService _authenticationService;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IApiKeyAuthenticationService authenticationService)
            : base(options, logger, encoder, clock)
        {
            _authenticationService = authenticationService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(AuthorizationHeaderName))
            {
                //Authorization header not in request
                return AuthenticateResult.NoResult();
            }

            if (!AuthenticationHeaderValue.TryParse(Request.Headers[AuthorizationHeaderName], out AuthenticationHeaderValue? headerValue))
            {
                //Invalid Authorization header
                return AuthenticateResult.NoResult();
            }

            if (!ApiKeySchemeName.Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                //Not ApiKey authentication header
                return AuthenticateResult.NoResult();
            }
            if (headerValue.Parameter is null)
            {
                //Missing key
                return AuthenticateResult.Fail("Missing apiKey");
            }

            bool isValid = await _authenticationService.IsValidAsync(headerValue.Parameter);

            if (!isValid)
            {
                return AuthenticateResult.Fail("Invalid apiKey");
            }
            var claims = new[] { new Claim(ClaimTypes.Name, "Service") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"X-API-KEY \", charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }
    }
}
