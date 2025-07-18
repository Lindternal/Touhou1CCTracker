using Touhou1CCTracker.Application.DTOs.Settings;
using Touhou1CCTracker.Application.Interfaces.Repositories;
using Touhou1CCTracker.Application.Interfaces.Services;
using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Application.Services;

public class SettingsService(ISettingsRepository settingsRepository) : ISettingsService
{
    public async Task<IEnumerable<SettingsResponseDto>> GetAllSettingsAsync()
    {
        var settings = await settingsRepository.GetAllSettingsAsync();
        return settings.Select(MapToResponseDto).ToList();
    }
    
    public async Task<string> GetSettingValueAsync(string settingName)
    {
        var setting = await settingsRepository.GetSettingByNameAsync(settingName);
        if (setting == null)
            throw new Exception($"Setting {settingName} not found!");
        
        return setting.SettingValue;
    }

    public async Task AddSettingAsync(SettingsCreateOrUpdateDto requestDto)
    {
        var setting = await settingsRepository.GetSettingByNameAsync(requestDto.SettingName);
        if (setting != null)
            throw new Exception($"Setting {requestDto.SettingName} already exists!");
        
        await settingsRepository.AddSettingAsync(
            new Settings
            {
                SettingName = requestDto.SettingName,
                SettingValue = requestDto.SettingValue
            }
        );

        await settingsRepository.SaveChangesAsync();
    }

    public async Task UpdateSettingAsync(SettingsCreateOrUpdateDto requestDto)
    {
        var setting = await settingsRepository.GetSettingByNameAsync(requestDto.SettingName);
        if (setting == null)
            throw new Exception($"Setting {requestDto.SettingName} not found!");
        
        setting.SettingValue = requestDto.SettingValue;
        
        await settingsRepository.SaveChangesAsync();
    }

    public async Task DeleteSettingAsync(string settingName)
    {
        var setting = await settingsRepository.GetSettingByNameAsync(settingName);
        if (setting == null)
            throw new Exception($"Setting {settingName} not found!");
        
        settingsRepository.DeleteSetting(setting);
        await settingsRepository.SaveChangesAsync();
    }

    private SettingsResponseDto MapToResponseDto(Settings setting)
    {
        return new SettingsResponseDto
        {
            Id = setting.Id,
            SettingName = setting.SettingName,
            SettingValue = setting.SettingValue
        };
    }
}