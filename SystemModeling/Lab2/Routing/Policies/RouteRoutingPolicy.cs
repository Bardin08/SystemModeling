using System.Collections.Concurrent;
using System.Threading.Channels;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Services;

namespace SystemModeling.Lab2.Routing.Policies;

internal class RouteRoutingPolicy<TEvent> : BaseRoutingPolicy<EventContext<TEvent>>
{
    private readonly RouteMappingService _routeMappingService;

    public RouteRoutingPolicy(
        RouteMappingService routeMappingService,
        ConcurrentQueue<EventContext<TEvent>> eventsStore,
        ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>> handlers)
        : base(eventsStore, handlers)
    {
        _routeMappingService = routeMappingService;
    }

    public override async Task RouteAsync(object? parameters, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            if (EventsStore.IsEmpty)
            {
                Task.Yield();
                continue;
            }

            if (EventsStore.TryDequeue(out var eventCtx))
            {
                var processorNode = _routeMappingService.GetProcessorNodeByName(eventCtx.NextProcessorName);

                if (processorNode.Name is "complete")
                {
                    // no need to route it. Processing complete
                    continue;                    
                }

                if (Handlers.TryGetValue(processorNode.RouteId, out var processor))
                {
                    await processor.WriteAsync(eventCtx, ct);
                }
                else
                {
                    throw new Exception($"Can't get processor for {eventCtx.NextProcessorName}");
                }

                var randomNumber = Random.Shared.NextDouble();
                var cumulative = 0d;
                foreach (var transition in processorNode.Transitions)
                {
                    cumulative += transition.TransitionChance;
                    if (cumulative >= randomNumber)
                    {
                        eventCtx.NextProcessorName = transition.ProcessorName;
                        break;
                    }
                }
            }
        }
    }
}