namespace Touhou1CCTracker.Domain.Events;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
}