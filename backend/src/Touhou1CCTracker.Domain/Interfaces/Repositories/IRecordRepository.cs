using Touhou1CCTracker.Domain.Entities;
using Touhou1CCTracker.Domain.Helpers;

namespace Touhou1CCTracker.Domain.Interfaces.Repositories;

public interface IRecordRepository
{
    Task<IEnumerable<Record>> GetAllRecordsAsync();
    Task<Record?> GetRecordByIdAsync(long id);
    Task<IEnumerable<Record>> GetRecordsByGameIdAsync(long gameId);
    Task<Record?> GetLatestRecordByGameAndDifficultyAsync(long gameId, long difficultyId);
    Task<PagedList<Record>> GetPagedLatestRecordsAsync(int pageNumber, int pageSize);
    Task AddRecordAsync(Record record);
    void DeleteRecord(Record record);
    Task<int> SaveChangesAsync();
}