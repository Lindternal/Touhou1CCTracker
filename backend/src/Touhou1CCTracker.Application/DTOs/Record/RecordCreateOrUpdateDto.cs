using Microsoft.AspNetCore.Http;

namespace Touhou1CCTracker.Application.DTOs.Record;

public class RecordCreateOrUpdateDto
{
    public string? Rank { get; set; }
    public long GameId { get; set; }
    public long DifficultyId { get; set; }
    public long ShotTypeId { get; set; }
    public DateOnly? Date { get; set; }
    public string? VideoUrl { get; set; }
}