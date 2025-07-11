namespace Touhou1CCTracker.Domain.Events;

public interface IEventHandler<TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent @event);
}