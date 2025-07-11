namespace Touhou1CCTracker.Domain.Entities;

public class Record
{
    public long Id { get; set; }
    public string? Rank { get; set; }
    public long GameId { get; set; }
    public Game Game { get; set; }
    public long DifficultyId { get; set; }
    public Difficulty Difficulty { get; set; }
    public long ShotTypeId { get; set; }
    public ShotType ShotType { get; set; }
    public DateOnly? Date { get; set; }
    public ReplayFile? ReplayFile { get; set; }
    public string? VideoUrl { get; set; }
}