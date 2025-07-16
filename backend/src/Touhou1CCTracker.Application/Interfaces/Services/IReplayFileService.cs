using Microsoft.AspNetCore.Mvc;
using Touhou1CCTracker.Application.DTOs.ReplayFile;

namespace Touhou1CCTracker.Application.Interfaces.Services;

public interface IReplayFileService
{
    Task UploadReplayFileAsync(ReplayFileUploadDto requestDto);
    Task<FileContentResult> GetDownloadLinkByRecordIdAsync(long recordId);
    Task DeleteReplayFileByIdAsync(long id);
    Task DeleteReplayFileByRecordIdAsync(long recordId);
}