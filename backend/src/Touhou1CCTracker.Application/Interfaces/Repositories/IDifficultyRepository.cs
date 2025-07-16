using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Application.Interfaces.Repositories;

public interface IDifficultyRepository
{
    Task<Difficulty?> GetDifficultyByIdAsync(long id);
    Task<bool> IsExistByIdAsync(long id);
    Task<bool> IsExistByNameAsync(string name);
    Task<IEnumerable<Difficulty>> GetAllDifficultiesAsync();
    Task<IEnumerable<Difficulty>> GetAllDifficultiesWithRecordsAsync();
    Task AddDifficultyAsync(Difficulty difficulty);
    void DeleteDifficulty(Difficulty difficulty);
    Task<int> SaveChangesAsync();
}