using Touhou1CCTracker.Application.DTOs.Game;

namespace Touhou1CCTracker.Application.Interfaces.Services;

public interface IGameService
{
    Task<GameResponseDto> CreateGameAsync(GameCreateOrUpdateDto requestDto);
    Task<GameResponseDto> GetGameByIdAsync(long id);
    Task<IEnumerable<GameResponseDto>> GetAllGamesAsync();
    Task<GameResponseDto> UpdateGameAsync(long id, GameCreateOrUpdateDto gameCreateOrUpdateDto);
    Task DeleteGameAsync(long id);
}