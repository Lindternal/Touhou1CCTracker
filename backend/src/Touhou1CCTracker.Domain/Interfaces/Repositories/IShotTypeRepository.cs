using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Domain.Interfaces.Repositories;

public interface IShotTypeRepository
{
    Task<ShotType?> GetShotTypeByIdAsync(long id);
    Task<bool> IsExistByIdAsync(long id);
    Task<bool> IsExistByNameAsync(string characterName, string shotName);
    Task<IEnumerable<ShotType?>> GetAllShotTypesAsync();
    Task<IEnumerable<ShotType?>> GetAllShotTypesByCharacterNameAsync(string characterName);
    Task<IEnumerable<string?>> GetAllCharacterNamesDistinctAsync();
    Task AddShotTypeAsync(ShotType shotType);
    void DeleteShotType(ShotType shotType);
    Task<int> SaveChangesAsync();
}