using System.Collections.Concurrent;
using System.Threading.Channels;
using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Policies;

namespace SystemModeling.Lab2.Routing.Services;

internal class EventsRoutingService<TEvent> : IEventsRoutingService<TEvent>
{
    private readonly ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>> _handlers;

    private readonly IRoutingPolicy _routingPolicy;

    public EventsRoutingService(
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        IRoutingMapService routingMapService)
    {
        _handlers = new ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>>();

        _routingPolicy = new RouteRoutingPolicy<TEvent>(
            routingMapService, eventStore, _handlers);
    }

    public bool AddRoute(string routeId, ChannelWriter<EventContext<TEvent>> channelWriter)
    {
        return _handlers.TryAdd(routeId, channelWriter);
    }

    public async Task RouteAsync(CancellationToken ct)
    {
        await _routingPolicy.RouteAsync(null, ct);
    }
}