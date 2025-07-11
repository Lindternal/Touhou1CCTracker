namespace Touhou1CCTracker.Domain.Entities;

public class ReplayFile
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public long Size { get; set; }
    public long RecordId { get; set; }
    public Record Record { get; set; }
}