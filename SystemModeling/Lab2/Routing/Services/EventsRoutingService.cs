using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Policies;

namespace SystemModeling.Lab2.Routing.Services;

internal class EventsRoutingService<TEvent> : IEventsRoutingService<TEvent>
{
    private readonly ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>> _handlers;

    private readonly IRoutingPolicy _routingPolicy;

    public EventsRoutingService(
        ChannelReader<EventContext<TEvent>> eventStoreReader,
        IRoutingMapService routingMapService)
    {
        _handlers = new ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>>();

        _routingPolicy = new RouteRoutingPolicy<TEvent>(
            routingMapService, eventStoreReader, _handlers);
    }

    public bool AddRoute(string routeId, ChannelWriter<EventContext<TEvent>> channelWriter)
    {
        return _handlers.TryAdd(routeId, channelWriter);
    }

    public Task RouteAsync(CancellationToken ct)
    {
        return _routingPolicy.RouteAsync(null, ct);
    }
}