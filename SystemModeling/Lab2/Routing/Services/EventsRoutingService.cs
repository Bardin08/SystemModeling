using SystemModeling.Lab2.ImitationCore.Observers;
using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Policies;

namespace SystemModeling.Lab2.Routing.Services;

internal class EventsRoutingService<TEvent> : IEventsRoutingService<TEvent>
{
    private readonly ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>> _handlers;

    private readonly IRoutingPolicy<TEvent> _routingPolicy;
    private readonly RoutingObserver<TEvent> _routingObserver;

    public EventsRoutingService(
        ChannelReader<EventContext<TEvent>> eventStoreReader,
        IRoutingMapService routingMapService)
    {
        _handlers = new ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>>();

        _routingPolicy = new PriorityRoutingPolicy<TEvent>(
            routingMapService, eventStoreReader, _handlers);

        _routingObserver = new RoutingObserver<TEvent>();
        _routingPolicy.RegisterObserver(_routingObserver);
    }

    public ProcessorRoutingDescriptor RoutingResultObserver(string processorName)
    {
        return _routingObserver.AddAndGetProcessorStats(processorName);
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