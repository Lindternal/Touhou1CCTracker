using Touhou1CCTracker.Application.DTOs.Settings;

namespace Touhou1CCTracker.Application.Interfaces.Services;

public interface ISettingsService
{
    Task<IEnumerable<SettingsResponseDto>> GetAllSettingsAsync();
    Task<string> GetSettingValueAsync(string settingName);
    Task AddSettingAsync(SettingsCreateOrUpdateDto requestDto);
    Task UpdateSettingAsync(SettingsCreateOrUpdateDto requestDto);
    Task DeleteSettingAsync(string settingName);
}