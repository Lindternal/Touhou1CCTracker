using Microsoft.EntityFrameworkCore;
using Touhou1CCTracker.Application.Interfaces.Repositories;
using Touhou1CCTracker.Domain.Entities;
using Touhou1CCTracker.Infrastructure.Data;

namespace Touhou1CCTracker.Infrastructure.Repositories;

public class DifficultyRepository(Touhou1CCTrackerDbContext context) : IDifficultyRepository
{
    public async Task<Difficulty?> GetDifficultyByIdAsync(long id)
    {
        return await context.Difficulties
            .Include(d => d.Records)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
    
    public async Task<bool> IsExistByIdAsync(long id)
    {
        return await context.Difficulties
            .AsNoTracking()
            .AnyAsync(s => s.Id == id);
    }
    
    public async Task<bool> IsExistByNameAsync(string name)
    {
        return await context.Difficulties
            .AsNoTracking()
            .AnyAsync(d => d.Name == name);
    }

    public async Task<IEnumerable<Difficulty>> GetAllDifficultiesAsync()
    {
        return await context.Difficulties
            .AsNoTracking()
            .OrderBy(d => d.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<Difficulty>> GetAllDifficultiesWithRecordsAsync()
    {
        return await context.Difficulties
            .Include(d => d.Records)
                .ThenInclude(r => r.Game)
            .Include(d => d.Records)
                .ThenInclude(r => r.ShotType)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddDifficultyAsync(Difficulty difficulty)
    {
        await context.Difficulties.AddAsync(difficulty);
    }

    public void DeleteDifficulty(Difficulty difficulty)
    {
        context.Difficulties.Remove(difficulty);
    }
    
    public async Task<int> SaveChangesAsync()
    { 
        return await context.SaveChangesAsync();
    }
}