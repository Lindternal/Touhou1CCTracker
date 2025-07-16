using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Application.Interfaces.Repositories;

public interface ISettingsRepository
{
    Task<IEnumerable<Settings>> GetAllSettingsAsync();
    Task<Settings?> GetSettingByNameAsync(string name);
    Task AddSettingAsync(Settings settings);
    void DeleteSetting(Settings settings);
    Task<int> SaveChangesAsync();
}