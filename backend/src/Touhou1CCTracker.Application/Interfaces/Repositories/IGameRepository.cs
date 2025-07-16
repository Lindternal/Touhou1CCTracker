using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Application.Interfaces.Repositories;

public interface IGameRepository
{
    Task<Game?> GetGameByIdAsync(long id);
    Task<bool> IsExistByIdAsync(long id);
    Task<bool> IsExistByNameAsync(string name);
    Task<IEnumerable<Game>> GetAllGamesAsync();
    Task<IEnumerable<Game>> GetAllGamesWithRecordsAsync();
    Task AddGameAsync(Game game);
    void DeleteGame(Game game);
    Task<int> SaveChangesAsync();
}