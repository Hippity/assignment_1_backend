using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBackend.Interaces;
using System.Threading.Tasks;

namespace MovieBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/")
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(HandleGoogleCallback)),
                Items = { { "returnUrl", returnUrl } }
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> HandleGoogleCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            
            if (!authenticateResult.Succeeded)
                return Unauthorized(new { message = "External authentication failed" });

            var token = await _accountService.GenerateJwtTokenAsync(authenticateResult.Principal);
            var userInfo = await _accountService.GetUserInfoFromExternalLoginAsync(authenticateResult.Principal);

            // Redirect to frontend with token (you may want to use a better approach for security)
            var returnUrl = authenticateResult.Properties.Items["returnUrl"] ?? "/";
            return Redirect($"{returnUrl}?token={token}");
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userInfo = await _accountService.GetUserInfoAsync(User);
            if (userInfo == null)
                return Unauthorized();
                
            return Ok(userInfo);
        }

        // A method to validate the token without requiring authorization
        [HttpPost("validate-token")]
        public IActionResult ValidateToken()
        {
            // If the request reaches here with a valid token, it means the token is valid
            return Ok(new { valid = true });
        }
    }
}