using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Touhou1CCTracker.Application.DTOs.Difficulty;
using Touhou1CCTracker.Application.Interfaces;

namespace Touhou1CCTracker.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class DifficultyController(IDifficultyService difficultyService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all difficulties",
        Description = "Returns all difficulties that exist in database."
        )]
    public async Task<ActionResult<DifficultyResponseDto>> GetAllDifficulties()
    {
        try
        {
            var difficulties = await difficultyService.GetAllDifficultiesAsync();
            return Ok(difficulties);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:long}")]
    [SwaggerOperation(
        Summary = "Get difficulty by provided ID",
        Description = "Returns difficulty by specified ID from database."
        )]
    public async Task<ActionResult<DifficultyResponseDto>> GetDifficultyById(long id)
    {
        try
        {
            var difficulty = await difficultyService.GetDifficultyByIdAsync(id);
            return Ok(difficulty);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Add difficulty",
        Description = "Creates a new difficulty in database by provided name. Returns this difficulty name with ID."
    )]
    public async Task<ActionResult<DifficultyResponseDto>> CreateDifficulty([FromBody] DifficultyCreateOrUpdateDto requestDto)
    {
        try
        {
            var difficulty = await difficultyService.CreateDifficultyAsync(requestDto);
            return Ok(difficulty);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:long}")]
    [SwaggerOperation(
        Summary = "Edit difficulty name",
        Description = "Edits the name of the difficulty by provided ID and new name. Returns this difficulty name with id."
    )]
    public async Task<ActionResult<DifficultyResponseDto>> UpdateDifficulty(long id,
        [FromBody] DifficultyCreateOrUpdateDto requestDto)
    {
        try
        {
            var difficulty = await difficultyService.UpdateDifficultyAsync(id, requestDto);
            return Ok(difficulty);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:long}")]
    [SwaggerOperation(
        Summary = "Delete difficulty",
        Description = "Deletes the difficulty from database by specified ID. Note: You can't delete difficulty if it has records!"
    )]
    public async Task<ActionResult> DeleteDifficulty(long id)
    {
        try
        {
            await difficultyService.DeleteDifficultyAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}