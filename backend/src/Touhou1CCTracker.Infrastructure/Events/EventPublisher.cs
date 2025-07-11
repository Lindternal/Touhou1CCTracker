using Microsoft.Extensions.DependencyInjection;
using Touhou1CCTracker.Domain.Events;

namespace Touhou1CCTracker.Infrastructure.Events;

public class EventPublisher(IServiceProvider serviceProvider) : IEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
    {
        var handlers = serviceProvider.GetServices<IEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            await handler.HandleAsync(@event);
        }
    }
}