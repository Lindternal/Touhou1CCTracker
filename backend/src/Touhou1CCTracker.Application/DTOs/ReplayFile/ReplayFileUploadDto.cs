using Microsoft.AspNetCore.Http;

namespace Touhou1CCTracker.Application.DTOs.ReplayFile;

public class ReplayFileUploadDto
{
    public long RecordId { get; set; }
    public IFormFile File { get; set; }
}