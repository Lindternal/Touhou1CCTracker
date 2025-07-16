using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Touhou1CCTracker.Application.DTOs.Settings;
using Touhou1CCTracker.Application.Interfaces.Services;

namespace Touhou1CCTracker.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingsController(ISettingsService settingsService) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all settings [Role = Admin]",
        Description = "Returns all settings from database with values."
    )]
    public async Task<ActionResult<SettingsResponseDto>> GetSettingsAsync()
    {
        try
        {
            var settings = await settingsService.GetAllSettingsAsync();
            return Ok(settings);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("settings")]
    [SwaggerOperation(
        Summary = "Change settings value [Role = Admin]",
        Description = "Change settings value by provided setting name and new value."
    )]
    public async Task<ActionResult> ChangeSettingsAsync([FromBody] SettingsCreateOrUpdateDto requestDto)
    {
        try
        {
            await settingsService.UpdateSettingAsync(requestDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}