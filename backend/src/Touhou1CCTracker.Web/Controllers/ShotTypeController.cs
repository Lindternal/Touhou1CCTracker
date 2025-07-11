using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Touhou1CCTracker.Application.DTOs.ShotType;
using Touhou1CCTracker.Application.Interfaces;
using Touhou1CCTracker.Application.Services;

namespace Touhou1CCTracker.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ShotTypeController(IShotTypeService shotTypeService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all shot types",
        Description = "Returns all shot types that exist in database."
        )]
    public async Task<ActionResult<ShotTypeResponseDto>> GetAllShotTypes()
    {
        try
        {
            var shotTypes = await shotTypeService.GetAllShotTypesAsync();
            return Ok(shotTypes);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:long}")]
    [SwaggerOperation(
        Summary = "Get shot type by provided ID",
        Description = "Returns shot type by specified ID from database."
        )]
    public async Task<ActionResult<ShotTypeResponseDto>> GetShotTypeById(long id)
    {
        try
        {
            var shotType = await shotTypeService.GetShotTypeByIdAsync(id);
            return Ok(shotType);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{characterName}")]
    [SwaggerOperation(
        Summary = "Get all shot types by provided character name",
        Description = "Returns all shot types form database by specified character name. Note: Case sensitive!"
        )]
    public async Task<ActionResult<ShotTypeResponseDto>> GetAllShotTypesByCharacterName(string characterName)
    {
        try
        {
            var shotTypes = await shotTypeService.GetAllShotTypesByCharacterNameAsync(characterName);
            return Ok(shotTypes);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Add shot type",
        Description = "Creates a new shot type in database by provided character name and shot name. Returns this shot type name with ID. Note: Shot name can be empty!"
        )]
    public async Task<ActionResult<ShotTypeResponseDto>> CreateShotType([FromBody] ShotTypeCreateOrUpdateDto requestDto)
    {
        try
        {
            var shotType = await shotTypeService.CreateShotTypeAsync(requestDto);
            return Ok(shotType);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:long}")]
    [SwaggerOperation(
        Summary = "Edit shot type name",
        Description = "Edits the shot types name by provided ID and new names. Returns this shot types name with ID."
        )]
    public async Task<ActionResult<ShotTypeResponseDto>> UpdateShotType(long id, [FromBody] ShotTypeCreateOrUpdateDto requestDto)
    {
        try
        {
            var shotType = await shotTypeService.UpdateShotTypeAsync(id, requestDto);
            return Ok(shotType);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:long}")]
    [SwaggerOperation(
        Summary = "Delete shot type",
        Description = "Deletes the shot type from database by specified ID. Note: You can't delete game if it has records!"
        )]
    public async Task<ActionResult> DeleteShotType(long id)
    {
        try
        {
            await shotTypeService.DeleteShotTypeAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}