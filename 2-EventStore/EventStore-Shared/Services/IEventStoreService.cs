using EventStore.Client;

namespace EventStore_Shared.Services;

public interface IEventStoreService
{
    Task AppendToStreamAsync(string streamName, IEnumerable<EventData> eventData);
    EventData GenerateEventData(object @event);
    Task SubscribeToStreamAsync(string streamName, Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> eventAppeared);
}