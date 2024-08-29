using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Welcome to API Gateway");
    }

    [AllowAnonymous]
    [HttpGet("login")]
    public IActionResult Login()
    {
        return Challenge(new AuthenticationProperties { RedirectUri = "https://localhost:4200" }, OpenIdConnectDefaults.AuthenticationScheme);
    }
    
    [Authorize]
    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        var claims = HttpContext.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
        return Ok(claims);
    }
    
    [HttpGet("logout")]
    public async Task Logout()
    {
        var prop = new AuthenticationProperties
        {
            RedirectUri = "/account/public"
        };
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, prop);

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}