using FluentValidation;
using Touhou1CCTracker.Application.DTOs.Record;
using Touhou1CCTracker.Application.Interfaces;
using Touhou1CCTracker.Domain.Entities;
using Touhou1CCTracker.Domain.Events;
using Touhou1CCTracker.Domain.Interfaces.Repositories;

namespace Touhou1CCTracker.Application.Services;

public class RecordService(IRecordRepository recordRepository,
    IGameRepository gameRepository,
    IDifficultyRepository difficultyRepository,
    IShotTypeRepository shotTypeRepository,
    IValidator<RecordCreateOrUpdateDto> validator,
    IEventPublisher eventPublisher) : IRecordService
{
    public async Task<RecordResponseDto> CreateRecordAsync(RecordCreateOrUpdateDto requestDto)
    {
        var validationResult = await validator.ValidateAsync(requestDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (!gameRepository.IsExistByIdAsync(requestDto.GameId).Result ||
            !difficultyRepository.IsExistByIdAsync(requestDto.DifficultyId).Result ||
            !shotTypeRepository.IsExistByIdAsync(requestDto.ShotTypeId).Result)
        {
            throw new Exception("One of the provided parameters are not exist!");
        }
        

        var record = new Record
        {
            Rank = requestDto.Rank.Trim(),
            GameId = requestDto.GameId,
            DifficultyId = requestDto.DifficultyId,
            ShotTypeId = requestDto.ShotTypeId,
            Date = requestDto.Date,
            VideoUrl = requestDto.VideoUrl
        };

        await recordRepository.AddRecordAsync(record);
        await recordRepository.SaveChangesAsync();
        
        return MapToResponseDto(await recordRepository.GetRecordByIdAsync(record.Id));
    }

    public async Task<IEnumerable<RecordResponseDto>> GetAllRecordsAsync()
    {
        var records = await recordRepository.GetAllRecordsAsync();
        return records.Select(MapToResponseDto).ToList();
    }

    public async Task<RecordResponseDto> GetRecordByIdAsync(long id)
    {
        var record = await recordRepository.GetRecordByIdAsync(id);
        if (record == null)
            throw new Exception("Record not found!");
        
        return MapToResponseDto(record);
    }

    public async Task<IEnumerable<RecordResponseDto>> GetRecordsByGameIdAsync(long gameId)
    {
        var records = await recordRepository.GetRecordsByGameIdAsync(gameId);
        if (records == null)
            throw new Exception("Record not found!");
        
        return records.Select(MapToResponseDto).ToList();
    }

    public async Task<RecordResponseDto> GetLatestRecordByGameAndDifficultyAsync(long gameId, long difficultyId)
    {
        var record = await recordRepository.GetLatestRecordByGameAndDifficultyAsync(gameId, difficultyId);
        if (record == null)
            throw new Exception("Record not found!");
        
        return MapToResponseDto(record);
    }

    public async Task<RecordPagedResponseDto> GetPagedLatestRecordsAsync(int page = 1, int pageSize = 20)
    {
        var records = await recordRepository.GetPagedLatestRecordsAsync(page, pageSize);
        if (records == null)
            throw new Exception("Record not found!");

        return new RecordPagedResponseDto()
        {
            Records = records.Select(MapToResponseDto).ToList(),
            TotalPages = records.TotalPages,
            CurrentPage = records.CurrentPage,
            PageSize = records.PageSize,
            TotalCount = records.TotalCount
        };
    }

    public async Task<RecordResponseDto> UpdateRecordAsync(long id, RecordCreateOrUpdateDto requestDto)
    {
        var validationResult = await validator.ValidateAsync(requestDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var record = await recordRepository.GetRecordByIdAsync(id);
        if (record == null)
            throw new Exception("Record not found!");
        
        if (!gameRepository.IsExistByIdAsync(requestDto.GameId).Result ||
            !difficultyRepository.IsExistByIdAsync(requestDto.DifficultyId).Result ||
            !shotTypeRepository.IsExistByIdAsync(requestDto.ShotTypeId).Result)
        {
            throw new Exception("One of the provided parameters are not exist!");
        }

        record.Rank = requestDto.Rank.Trim();
        record.GameId = requestDto.GameId;
        record.DifficultyId = requestDto.DifficultyId;
        record.ShotTypeId = requestDto.ShotTypeId;
        record.Date = requestDto.Date;
        record.VideoUrl = requestDto.VideoUrl;
        
        await recordRepository.SaveChangesAsync();
        
        return MapToResponseDto(await recordRepository.GetRecordByIdAsync(id));
    }

    public async Task DeleteRecordAsync(long id)
    {
        var record = await recordRepository.GetRecordByIdAsync(id);
        if (record == null)
            throw new Exception("Record not found!");

        long replayFileId = 0;
        if (record.ReplayFile != null)
            replayFileId = record.ReplayFile.Id;
        
        if (replayFileId != 0)
            await eventPublisher.PublishAsync(new RecordDeletedEvent(replayFileId));

        recordRepository.DeleteRecord(record);
        
        await recordRepository.SaveChangesAsync();
    }
    
    private RecordResponseDto MapToResponseDto(Record record)
    {
        bool hasFile = record.ReplayFile != null ? true : false;
        return new RecordResponseDto()
        {
            Id = record.Id,
            Rank = record.Rank,
            GameId = record.GameId,
            GameName = record.Game.Name,
            DifficultyId = record.DifficultyId,
            DifficultyName = record.Difficulty.Name,
            ShotTypeId = record.ShotTypeId,
            CharacterName = record.ShotType.CharacterName,
            ShotName = record.ShotType.ShotName,
            Date = record.Date,
            VideoUrl = record.VideoUrl,
            HasReplayFile = hasFile,
        };
    }
}