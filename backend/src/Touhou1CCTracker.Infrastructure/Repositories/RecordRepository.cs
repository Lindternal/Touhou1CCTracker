using Microsoft.EntityFrameworkCore;
using Touhou1CCTracker.Application.Interfaces.Repositories;
using Touhou1CCTracker.Domain.Entities;
using Touhou1CCTracker.Domain.Helpers;
using Touhou1CCTracker.Infrastructure.Data;

namespace Touhou1CCTracker.Infrastructure.Repositories;

public class RecordRepository(Touhou1CCTrackerDbContext context) : IRecordRepository
{
    public async Task<IEnumerable<Record>> GetAllRecordsAsync()
    {
        return await context.Records
            .AsNoTracking()
            .Include(r => r.Game)
            .Include(r => r.Difficulty)
            .Include(r => r.ShotType)
            .Include(r => r.ReplayFile)
            .ToListAsync();
    }

    public async Task<Record?> GetRecordByIdAsync(long id)
    {
        return await context.Records
            .Include(r => r.Game)
            .Include(r => r.Difficulty)
            .Include(r => r.ShotType)
            .Include(r => r.ReplayFile)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Record>> GetRecordsByGameIdAsync(long gameId)
    {
        return await context.Records
            .AsNoTracking()
            .Include(r => r.Game)
            .Include(r => r.Difficulty)
            .Include(r => r.ShotType)
            .Include(r => r.ReplayFile)
            .Where(r => r.GameId == gameId)
            .OrderByDescending(r => r.Date)
            .ThenByDescending(r => r.Id)
            .ToListAsync();
    }

    public async Task<Record?> GetLatestRecordByGameAndDifficultyAsync(long gameId, long difficultyId)
    {
        return await context.Records
            .AsNoTracking()
            .Include(r => r.Game)
            .Include(r => r.Difficulty)
            .Include(r => r.ShotType)
            .Include(r => r.ReplayFile)
            .Where(r => r.GameId == gameId && r.DifficultyId == difficultyId)
            .OrderByDescending(r => r.Date)
            .FirstOrDefaultAsync();
    }

    public async Task<PagedList<Record>> GetPagedLatestRecordsAsync(
        int pageNumber = 1,
        int pageSize = 20)
    {
        var source = context.Records
            .AsNoTracking()
            .Include(r => r.Game)
            .Include(r => r.Difficulty)
            .Include(r => r.ShotType)
            .Include(r => r.ReplayFile)
            .OrderByDescending(r => r.Date)
            .ThenByDescending(r => r.Id);
        
        return await PagedList<Record>.CreateAsync(source, pageNumber, pageSize);
    }

    public async Task AddRecordAsync(Record record)
    {
        await context.Records.AddAsync(record);
    }

    public void DeleteRecord(Record record)
    {
        context.Records.Remove(record);
    }
    
    public async Task<int> SaveChangesAsync()
    { 
        return await context.SaveChangesAsync();
    }
}