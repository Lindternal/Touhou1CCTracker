using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Touhou1CCTracker.Application.DTOs.ReplayFile;
using Touhou1CCTracker.Application.Interfaces;
using Touhou1CCTracker.Domain.Entities;
using Touhou1CCTracker.Domain.Interfaces.Repositories;

namespace Touhou1CCTracker.Application.Services;

public class ReplayFileService(IReplayFileRepository replayFileRepository,
    IRecordRepository recordRepository,
    IValidator<ReplayFileUploadDto> validator) : IReplayFileService
{
    public async Task UploadReplayFileAsync(ReplayFileUploadDto requestDto)
    {
        var validationResult = await validator.ValidateAsync(requestDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var record = await recordRepository.GetRecordByIdAsync(requestDto.RecordId);
        if (record == null)
            throw new Exception("Record not found!");
        
        if (record.ReplayFile != null)
            throw new Exception("Replay file already exists!");

        var fileNameWithTimestamp = GenerateFileNameWithTimestamp(requestDto.File.FileName);
        
        var fullPath = Path.Combine(GetUploadsPath(), fileNameWithTimestamp);
        
        if (File.Exists(fullPath))
            throw new Exception($"File is already exists!");

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await requestDto.File.CopyToAsync(stream);

        var replayFile = new ReplayFile
        {
            Name = requestDto.File.FileName,
            Path = fileNameWithTimestamp,
            Size = requestDto.File.Length,
            RecordId = record.Id
        };
        
        await replayFileRepository.AddReplayFileAsync(replayFile);
        await replayFileRepository.SaveChangesAsync();
    }

    public async Task<FileContentResult> GetDownloadLinkByRecordIdAsync(long recordId)
    {
        var record = await recordRepository.GetRecordByIdAsync(recordId);
        if (record == null)
            throw new Exception("Record not found!");
        
        if (record.ReplayFile == null)
            throw new Exception("File not found!");
        
        var file = await replayFileRepository.GetReplayFileByIdAsync(record.ReplayFile.Id);
        if (file == null)
            throw new Exception("File not found!");
        
        var completePath = Path.Combine(GetUploadsPath(), file.Path);
        
        if (!File.Exists(completePath))
            throw new Exception($"File is not exists!");
        
        var fileBytes = await File.ReadAllBytesAsync(completePath);
        var fileContentResult = new FileContentResult(fileBytes, "application/octet-stream")
        {
            FileDownloadName = file.Name
        };

        return fileContentResult;
    }

    public async Task DeleteReplayFileByIdAsync(long id)
    {
        var file = await replayFileRepository.GetReplayFileByIdAsync(id);
        if (file == null)
            throw new Exception("File not found!");
        
        var completePath = Path.Combine(GetUploadsPath(), file.Path);
        
        if (!File.Exists(completePath))
            throw new Exception($"File is not exists!");
        
        File.Delete(completePath);
        
        replayFileRepository.DeleteReplayFile(file);
        
        await replayFileRepository.SaveChangesAsync();
    }

    public async Task DeleteReplayFileByRecordIdAsync(long recordId)
    {
        var record = await recordRepository.GetRecordByIdAsync(recordId);
        if (record == null)
            throw new Exception("Record not found!");
        
        if (record.ReplayFile == null)
            throw new Exception("File not found!");
        
        var file = await replayFileRepository.GetReplayFileByIdAsync(record.ReplayFile.Id);
        if (file == null)
            throw new Exception("File not found!");
        
        var completePath = Path.Combine(GetUploadsPath(), file.Path);
        
        if (!File.Exists(completePath))
            throw new Exception($"File is not exists!");
        
        File.Delete(completePath);
        
        replayFileRepository.DeleteReplayFile(file);
        
        await replayFileRepository.SaveChangesAsync();
    }

    private string GenerateFileNameWithTimestamp(string originalFileName)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
        var extension = Path.GetExtension(originalFileName);
        
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        
        return $"{fileNameWithoutExtension}-{timestamp}{extension}";
    }

    private string GetUploadsPath()
    {
        var currentDir = Directory.GetCurrentDirectory();
        var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\"));
        var uploadDir = Path.Combine(projectRoot, "uploads");
        
        if (!Directory.Exists(uploadDir))
            Directory.CreateDirectory(uploadDir);
        
        return uploadDir;
    }
}