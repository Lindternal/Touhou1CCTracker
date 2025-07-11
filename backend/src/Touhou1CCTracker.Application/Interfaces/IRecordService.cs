using Touhou1CCTracker.Application.DTOs.Record;

namespace Touhou1CCTracker.Application.Interfaces;

public interface IRecordService
{
    Task<RecordResponseDto> CreateRecordAsync(RecordCreateOrUpdateDto requestDto);
    Task<IEnumerable<RecordResponseDto>> GetAllRecordsAsync();
    Task<RecordResponseDto> GetRecordByIdAsync(long id);
    Task<IEnumerable<RecordResponseDto>> GetRecordsByGameIdAsync(long gameId);
    Task<RecordResponseDto> GetLatestRecordByGameAndDifficultyAsync(long gameId, long difficultyId);
    Task<RecordPagedResponseDto> GetPagedLatestRecordsAsync(int page, int pageSize);
    Task<RecordResponseDto> UpdateRecordAsync(long id, RecordCreateOrUpdateDto requestDto);
    Task DeleteRecordAsync(long id);
}