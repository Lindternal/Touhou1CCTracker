namespace Touhou1CCTracker.Application.DTOs.Record;

public class RecordResponseDto
{
    public long Id { get; set; }
    public string Rank { get; set; }
    public long GameId { get; set; }
    public string GameName { get; set; }
    public long DifficultyId { get; set; }
    public string DifficultyName { get; set; }
    public long ShotTypeId { get; set; }
    public string CharacterName { get; set; }
    public string? ShotName { get; set; }
    public DateOnly? Date { get; set; }
    public string? VideoUrl { get; set; }
    public bool HasReplayFile { get; set; } = false;
}