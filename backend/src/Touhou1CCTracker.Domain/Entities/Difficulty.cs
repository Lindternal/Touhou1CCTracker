namespace Touhou1CCTracker.Domain.Entities;

public class Difficulty
{
    public long Id { get; set; }
    public string Name { get; set; }
    public ICollection<Record> Records { get; set; } = new List<Record>();
}