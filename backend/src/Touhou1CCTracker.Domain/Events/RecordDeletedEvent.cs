namespace Touhou1CCTracker.Domain.Events;

public class RecordDeletedEvent(long replayFileId) : IEvent
{
    public long ReplayFileId { get; } = replayFileId;
}