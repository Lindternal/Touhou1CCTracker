using Touhou1CCTracker.Application.Interfaces;
using Touhou1CCTracker.Application.Interfaces.Services;
using Touhou1CCTracker.Domain.Events;

namespace Touhou1CCTracker.Application.Events;

public class DeleteReplayFileHandler(IReplayFileService replayFileService) : IEventHandler<RecordDeletedEvent>
{
    public async Task HandleAsync(RecordDeletedEvent @event)
    {
        await replayFileService.DeleteReplayFileByIdAsync(@event.ReplayFileId);
    }
}