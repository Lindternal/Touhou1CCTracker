using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Touhou1CCTracker.Application.DTOs.Game;
using Touhou1CCTracker.Application.Interfaces;

namespace Touhou1CCTracker.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all games",
        Description = "Returns all games that exist in database."
        )]
    public async Task<ActionResult<GameResponseDto>> GetAllGames()
    {
        try
        {
            var games = await gameService.GetAllGamesAsync();
            return Ok(games);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("{id:long}")]
    [SwaggerOperation(
        Summary = "Get game by provided ID",
        Description = "Returns game by specified ID from database."
        )]
    public async Task<ActionResult<GameResponseDto>> GetGameById(long id)
    {
        try
        {
            var game = await gameService.GetGameByIdAsync(id);
            return Ok(game);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Add game",
        Description = "Creates a new game in database by provided name. Returns this game name with ID."
        )]
    public async Task<ActionResult<GameResponseDto>> CreateGame([FromBody] GameCreateOrUpdateDto requestDto)
    {
        try
        {
            var game = await gameService.CreateGameAsync(requestDto);
            return Ok(game);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:long}")]
    [SwaggerOperation(
        Summary = "Edit game name",
        Description = "Edits the name of the game by provided ID and new name. Returns this game name with id."
        )]
    public async Task<ActionResult<GameResponseDto>> UpdateGame(long id, [FromBody] GameCreateOrUpdateDto requestDto)
    {
        try
        {
            var game = await gameService.UpdateGameAsync(id, requestDto);
            return Ok(game);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:long}")]
    [SwaggerOperation(
        Summary = "Delete game",
        Description = "Deletes the game from database by specified ID. Note: You can't delete game if it has records!"
        )]
    public async Task<ActionResult> DeleteGame(long id)
    {
        try
        {
            await gameService.DeleteGameAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}