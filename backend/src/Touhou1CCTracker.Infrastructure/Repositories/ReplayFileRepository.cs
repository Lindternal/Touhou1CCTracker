using Microsoft.EntityFrameworkCore;
using Touhou1CCTracker.Domain.Entities;
using Touhou1CCTracker.Domain.Interfaces.Repositories;
using Touhou1CCTracker.Infrastructure.Data;

namespace Touhou1CCTracker.Infrastructure.Repositories;

public class ReplayFileRepository(Touhou1CCTrackerDbContext context) : IReplayFileRepository
{
    public async Task<ReplayFile?> GetReplayFileByIdAsync(long id)
    {
        return await context.ReplayFiles
            .Include(f => f.Record)
            .FirstOrDefaultAsync(f => f.Id == id);
    }
    
    public async Task AddReplayFileAsync(ReplayFile replayFile)
    {
        await context.AddAsync(replayFile); 
    }

    public void DeleteReplayFile(ReplayFile replayFile)
    {
        context.Remove(replayFile);
    }
    
    public async Task<int> SaveChangesAsync()
    { 
        return await context.SaveChangesAsync();
    }
}