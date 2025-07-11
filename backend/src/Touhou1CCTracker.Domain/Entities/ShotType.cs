namespace Touhou1CCTracker.Domain.Entities;

public class ShotType
{
    public long Id { get; set; }
    public string CharacterName { get; set; }
    public string? ShotName { get; set; }
    public ICollection<Record> Records { get; set; } = new List<Record>();
}