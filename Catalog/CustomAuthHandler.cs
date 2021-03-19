using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Catalog
{
    public class CustomAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder urlEncoder, ISystemClock clock) 
            :base(options, logger, urlEncoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var identity = new ClaimsIdentity("CustomAuthType");
            identity.AddClaim(new Claim(ClaimTypes.Name, "TestUser-" + Guid.NewGuid().ToString()));
            var principal = new ClaimsPrincipal(identity);
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, nameof(CustomAuthHandler))));
        }
    }
}
