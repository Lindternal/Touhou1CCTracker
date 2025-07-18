using Microsoft.EntityFrameworkCore;
using Touhou1CCTracker.Application.Interfaces.Repositories;
using Touhou1CCTracker.Domain.Entities;
using Touhou1CCTracker.Infrastructure.Data;

namespace Touhou1CCTracker.Infrastructure.Repositories;

public class ShotTypeRepository(Touhou1CCTrackerDbContext context) : IShotTypeRepository
{
    public async Task<ShotType?> GetShotTypeByIdAsync(long id)
    {
        return await context.ShotTypes
            .Include(s => s.Records)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<bool> IsExistByIdAsync(long id)
    {
        return await context.ShotTypes
            .AsNoTracking()
            .AnyAsync(s => s.Id == id);
    }

    public async Task<bool> IsExistByNameAsync(string characterName, string shotName)
    {
        return await context.ShotTypes
            .AsNoTracking()
            .AnyAsync(s => s.CharacterName == characterName && s.ShotName == shotName);
    }
    
    public async Task<IEnumerable<ShotType?>> GetAllShotTypesAsync()
    {
        return await context.ShotTypes
            .AsNoTracking()
            .OrderBy(s => s.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<ShotType?>> GetAllShotTypesByCharacterNameAsync(string characterName)
    {
        return await context.ShotTypes
            .AsNoTracking()
            .Where(s => s.CharacterName == characterName)
            .ToListAsync();
    }

    public async Task<IEnumerable<string?>> GetAllCharacterNamesDistinctAsync()
    {
        return await context.ShotTypes
            .Select(s => s.CharacterName)
            .Distinct()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddShotTypeAsync(ShotType shotType)
    {
        await context.ShotTypes.AddAsync(shotType);
    }

    public void DeleteShotType(ShotType shotType)
    {
        context.ShotTypes.Remove(shotType);
    }
    
    public async Task<int> SaveChangesAsync()
    { 
        return await context.SaveChangesAsync();
    }
}