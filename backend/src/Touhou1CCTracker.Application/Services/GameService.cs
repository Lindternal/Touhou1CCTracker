using FluentValidation;
using Touhou1CCTracker.Application.DTOs.Game;
using Touhou1CCTracker.Application.Interfaces.Repositories;
using Touhou1CCTracker.Application.Interfaces.Services;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Application.Services;

public class GameService(IGameRepository gameRepository, IValidator<GameCreateOrUpdateDto> validator) : IGameService
{
    public async Task<GameResponseDto> CreateGameAsync(GameCreateOrUpdateDto requestDto)
    {
        var validationResult = await validator.ValidateAsync(requestDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (await gameRepository.IsExistByNameAsync(requestDto.Name))
            throw new Exception($"Game with name \"{requestDto.Name}\" already exists!");

        var game = new Game
        {
            Name = requestDto.Name.Trim()
        };

        await gameRepository.AddGameAsync(game);
        await gameRepository.SaveChangesAsync();

        return MapToResponseDto(game);
    }

    public async Task<GameResponseDto> GetGameByIdAsync(long id)
    {
        var game = await gameRepository.GetGameByIdAsync(id);
        if (game == null)
            throw new Exception($"Game with id \"{id}\" does not exist!");
        
        return MapToResponseDto(game);
    }

    public async Task<IEnumerable<GameResponseDto>> GetAllGamesAsync()
    {
        var games = await gameRepository.GetAllGamesAsync();
        return games.Select(MapToResponseDto).ToList();
    }

    public async Task<GameResponseDto> UpdateGameAsync(long id, GameCreateOrUpdateDto gameCreateOrUpdateDto)
    {
        var validationResult = await validator.ValidateAsync(gameCreateOrUpdateDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var game = await gameRepository.GetGameByIdAsync(id);
        if (game == null)
            throw new Exception($"Game with id \"{id}\" does not exist!");
        
        if (await gameRepository.IsExistByNameAsync(gameCreateOrUpdateDto.Name))
            throw new Exception($"Game with name \"{gameCreateOrUpdateDto.Name}\" already exists!");
        
        game.Name = gameCreateOrUpdateDto.Name.Trim();

        await gameRepository.SaveChangesAsync();

        return MapToResponseDto(game);
    }

    public async Task DeleteGameAsync(long id)
    {
        var game = await gameRepository.GetGameByIdAsync(id);
        if (game == null)
            throw new Exception($"Game with id \"{id}\" does not exist!");
        
        if (game.Records.Any())
            throw new Exception($"Can not delete game \"{game.Name}\" because it has records!");
        
        gameRepository.DeleteGame(game);
        
        await gameRepository.SaveChangesAsync();
    }

    private GameResponseDto MapToResponseDto(Game game)
    {
        return new GameResponseDto
        {
            Id = game.Id,
            Name = game.Name
        };
    }
}