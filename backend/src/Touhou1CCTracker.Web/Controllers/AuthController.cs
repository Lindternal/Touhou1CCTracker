using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Touhou1CCTracker.Application.DTOs.Login;
using Touhou1CCTracker.Application.Interfaces.Services;

namespace Touhou1CCTracker.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService,
    ISettingsService settingsService,
    IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "Registers user [Role = Any]",
        Description = "Add new user with unique username to database."
    )]
    public async Task<ActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
    {
        try
        {
            var isRegistrationEnabled = await settingsService.GetSettingValueAsync("IsRegistrationEnabled");
            
            switch (isRegistrationEnabled)
            {
                case "false":
                    return BadRequest("Registration is disabled!");
                case "true":
                    await authService.RegisterAsync(registerDto);
                    return Ok();
                default:
                    throw new Exception($"Settings value {isRegistrationEnabled} is not supported!");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Login [Role = Any]",
        Description = "Login existing user."
    )]
    public async Task<ActionResult<LoginResponseDto>> LoginAsync([FromBody] LoginRequestDto requestDto)
    {
        try
        {
            var tuple = await authService.LoginAsync(requestDto);
            Response.Cookies.Append(configuration.GetValue<string>("AuthSettings:CookieName"), tuple.Item2, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(configuration.GetValue<int>("AuthSettings:ExpiresHours")),
                //Secure = true,
                //Domain = configuration.GetValue<string>("AuthSettings:Domain")
            });
            return Ok(tuple.Item1);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("logout")]
    [SwaggerOperation(
        Summary = "Logout [Role = Any]",
        Description = "Logout user."
    )]
    public Task<ActionResult> LogoutAsync()
    {
        try
        {
            Response.Cookies.Delete(configuration.GetValue<string>("AuthSettings:CookieName"));

            return Task.FromResult<ActionResult>(Ok( new { message = "Logout successful." }));
        }
        catch (Exception ex)
        {
            return Task.FromResult<ActionResult>(BadRequest(ex.Message));
        }
    }

    [Authorize]
    [HttpGet("check")]
    [SwaggerOperation(
        Summary = "Check if user logged in [Role = Any]",
        Description = "Check if user logged in. Returns user information."
    )]
    public Task<ActionResult> CheckLogin()
    {
        try
        {
            var userName = User.FindFirst("userName")?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            
            return Task.FromResult<ActionResult>(Ok(new { userName = userName, role = role }));
        }
        catch (Exception ex)
        {
            return Task.FromResult<ActionResult>(BadRequest(ex.Message));
        }
    }
}