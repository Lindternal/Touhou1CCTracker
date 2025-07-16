using Microsoft.EntityFrameworkCore;
using Touhou1CCTracker.Application.Interfaces.Repositories;
using Touhou1CCTracker.Domain.Entities;
using Touhou1CCTracker.Infrastructure.Data;

namespace Touhou1CCTracker.Infrastructure.Repositories;

public class SettingsRepository(Touhou1CCTrackerDbContext context) : ISettingsRepository
{
    public async Task<IEnumerable<Settings>> GetAllSettingsAsync()
    {
        return await context.Settings
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<Settings?> GetSettingByNameAsync(string name)
    {
        return await context.Settings
            .FirstOrDefaultAsync(s => s.SettingName == name);
    }
    
    public async Task AddSettingAsync(Settings settings)
    {
        await context.Settings.AddAsync(settings);
    }

    public void DeleteSetting(Settings settings)
    {
        context.Settings.Remove(settings);
    }
    
    public async Task<int> SaveChangesAsync()
    { 
        return await context.SaveChangesAsync();
    }
}