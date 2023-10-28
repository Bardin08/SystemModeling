using System.Collections.Concurrent;
using System.Threading.Channels;
using SystemModeling.Lab2.Routing.Policies;

namespace SystemModeling.Lab2.Routing;

public class EventRouter<TEvent>
{
    private readonly ConcurrentDictionary<string, ChannelWriter<TEvent>> _handlers;

    private readonly IRoutingPolicy _routingPolicy;

    public EventRouter(ConcurrentQueue<TEvent> eventStore)
    {
        _handlers = new ConcurrentDictionary<string, ChannelWriter<TEvent>>();

        _routingPolicy = new EquallyDistributionRoutingPolicy<TEvent>(eventStore, _handlers);
    }

    public bool AddRoute(string routeId, ChannelWriter<TEvent> channelWriter)
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