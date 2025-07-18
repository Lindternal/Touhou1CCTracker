using Touhou1CCTracker.Domain.Entities;

namespace Touhou1CCTracker.Application.Interfaces.Repositories;

public interface IReplayFileRepository
{
    Task<ReplayFile?> GetReplayFileByIdAsync(long id);
    Task AddReplayFileAsync(ReplayFile replayFile);
    void DeleteReplayFile(ReplayFile replayFile);
    Task<int> SaveChangesAsync();
}