using System.Collections.Concurrent;
using System.Threading.Channels;
using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Routing.Policies;

internal abstract class BaseRoutingPolicy<TEvent> : IRoutingPolicy
{
    protected readonly ChannelReader<EventContext<TEvent>> EventsStore;
    protected readonly ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>> Handlers;

    protected BaseRoutingPolicy(
        ChannelReader<EventContext<TEvent>> eventsStore,
        ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>> handlers)
    {
        EventsStore = eventsStore;
        Handlers = handlers;
    }

    public abstract Task RouteAsync(object? parameters, CancellationToken ct);
}