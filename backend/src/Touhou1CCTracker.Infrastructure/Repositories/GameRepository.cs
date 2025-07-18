using Microsoft.EntityFrameworkCore;
using Touhou1CCTracker.Application.Interfaces.Repositories;
using Touhou1CCTracker.Domain.Entities;
using Touhou1CCTracker.Infrastructure.Data;

namespace Touhou1CCTracker.Infrastructure.Repositories;

public class GameRepository(Touhou1CCTrackerDbContext context) : IGameRepository
{
    public async Task<Game?> GetGameByIdAsync(long id)
    {
        return await context.Games
            .Include(g => g.Records)
            .FirstOrDefaultAsync(g => g.Id == id);
    }
    
    public async Task<bool> IsExistByIdAsync(long id)
    {
        return await context.Games
            .AsNoTracking()
            .AnyAsync(g => g.Id == id);
    }
    
    public async Task<bool> IsExistByNameAsync(string name)
    {
        return await context.Games
            .AsNoTracking()
            .AnyAsync(g => g.Name == name);
    }
    
    public async Task<IEnumerable<Game>> GetAllGamesAsync()
    {
        return await context.Games
            .AsNoTracking()
            .OrderBy(g => g.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<Game>> GetAllGamesWithRecordsAsync()
    {
        return await context.Games
            .Include(g => g.Records)
                .ThenInclude(r => r.Difficulty)
            .Include(g => g.Records)
                .ThenInclude(r => r.ShotType)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddGameAsync(Game game)
    {
        await context.Games.AddAsync(game);
    }

    public void DeleteGame(Game game)
    {
        context.Games.Remove(game);
    }
    
    public async Task<int> SaveChangesAsync()
    { 
        return await context.SaveChangesAsync();
    }
}