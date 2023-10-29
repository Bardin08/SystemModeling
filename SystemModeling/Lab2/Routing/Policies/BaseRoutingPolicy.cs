using System.Collections.Concurrent;
using System.Threading.Channels;
using SystemModeling.Lab2.Routing.Interfaces;

namespace SystemModeling.Lab2.Routing.Policies;

internal abstract class BaseRoutingPolicy<TEvent> : IRoutingPolicy
{
    protected readonly ConcurrentQueue<TEvent> EventsStore;
    protected readonly ConcurrentDictionary<string, ChannelWriter<TEvent>> Handlers;

    protected BaseRoutingPolicy(
        ConcurrentQueue<TEvent> eventsStore,
        ConcurrentDictionary<string, ChannelWriter<TEvent>> handlers)
    {
        EventsStore = eventsStore;
        Handlers = handlers;
    }

    public abstract Task RouteAsync(object? parameters, CancellationToken ct);
}