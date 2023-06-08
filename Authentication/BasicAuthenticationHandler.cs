using AMSAPI.Services;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.Extensions.Hosting;
using AMSAPI.Models;

namespace AMSAPI.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;
        private IHostEnvironment _hostEnvironment;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserService userService, IHostEnvironment hostEnvironment)
            : base(options, logger, encoder, clock)
        {
            _hostEnvironment = hostEnvironment;
             _userService = userService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
            }

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (authHeader.Scheme != "Basic")
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header scheme"));
            }

            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
            var username = credentials[0];
            var password = credentials[1];

            //make sure header has clientcode
            if (!Request.Headers.ContainsKey("ClientCode"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Client"));
            }

            User? user =  _userService.Authenticate(username, password, Request.Headers["ClientCode"], _hostEnvironment);
            if (user == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));
            }
            else if(!user.IsValid??true)
            {
                return Task.FromResult(AuthenticateResult.Fail(user.Error??"Invalid User or Client"));
            }
           

            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), "userid"),
            new Claim(ClaimTypes.Name, user.Username??"","username"),
            new Claim(ClaimTypes.SerialNumber,user.GoogleSheetId??"","googlesheetid")
            };
            
            var authProps = new AuthenticationProperties();
            authProps.Items.Add("googlesheetid", user.GoogleSheetId);
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, authProps,Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
