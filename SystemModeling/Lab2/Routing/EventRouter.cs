using System.Collections.Concurrent;
using System.Threading.Channels;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Policies;
using SystemModeling.Lab2.Routing.Services;

namespace SystemModeling.Lab2.Routing;

internal class EventRouter<TEvent>
{
    private readonly ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>> _handlers;

    private readonly IRoutingPolicy _routingPolicy;

    public EventRouter(
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        RouteMappingService routeMappingService)
    {
        _handlers = new ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>>();

        _routingPolicy = new RouteRoutingPolicy<TEvent>(
            routeMappingService, eventStore, _handlers);
    }

    public bool AddRoute(string routeId, ChannelWriter<EventContext<TEvent>> channelWriter)
    {
        return _handlers.TryAdd(routeId, channelWriter);
    }

    public bool RemoveRoute(string routeId)
    {
        if (!_handlers.TryGetValue(routeId, out var channelWriter))
        {
            return false;
        }

        channelWriter.Complete();
        return _handlers.TryRemove(routeId, out _);
    }

    public async Task RouteAsync(CancellationToken ct)
    {
        await _routingPolicy.RouteAsync(null, ct);
    }
}