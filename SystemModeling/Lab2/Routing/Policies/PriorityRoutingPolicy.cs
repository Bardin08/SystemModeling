using SystemModeling.Lab2.ImitationCore.Observers;
using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Routing.Policies;

internal class PriorityRoutingPolicy<TEvent> : BaseRoutingPolicy<TEvent>
{
    private readonly IRoutingMapService _routingMapService;

    public PriorityRoutingPolicy(
        IRoutingMapService routingMapService,
        ChannelReader<EventContext<TEvent>> eventsStore,
        ConcurrentDictionary<string, ChannelWriter<EventContext<TEvent>>> handlers)
        : base(eventsStore, handlers)
    {
        _routingMapService = routingMapService;
    }

    public override Task RouteAsync(object? parameters, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            if (EventsStore.Count is 0)
            {
                Task.Yield();
                continue;
            }

            if (!EventsStore.TryRead(out var eventCtx))
            {
                continue;
            }

            var processorNode = _routingMapService
                .GetProcessorNodeByName(eventCtx.NextProcessorName!);

            // no need to route it. Processing complete
            if (processorNode?.Name is "complete")
            {
                continue;
            }

            if (processorNode?.RouteId is null)
            {
                continue;
            }

            if (Handlers.TryGetValue(processorNode.RouteId, out var processor))
            {
                RoutingResult<TEvent>? routingResult;
                if (processor.TryWrite(eventCtx))
                    routingResult = new RoutingResult<TEvent>
                    {
                        IsSuccess = true,
                        EventContext = eventCtx,
                        TargetedProcessor = processorNode.Name ?? "name not defined"
                    };
                else
                    routingResult = new RoutingResult<TEvent>
                    {
                        IsSuccess = false,
                        EventContext = eventCtx,
                        TargetedProcessor = processorNode.Name ?? "name not defined"
                    };

                Notify(routingResult);
            }
            else
            {
                throw new Exception($"Can't get processor for {eventCtx.NextProcessorName}");
            }

            var randomNumber = Random.Shared.NextDouble();
            var cumulative = 0d;
            var transitions = processorNode
                .Transitions
                .UnorderedItems
                .Select(x => x.Element);
            foreach (var transition in transitions)
            {
                ArgumentNullException.ThrowIfNull(transition.TransitionChance);

                cumulative += transition.TransitionChance.Value;
                if (!(cumulative >= randomNumber)) continue;

                eventCtx.NextProcessorName = transition.ProcessorName;
                break;
            }
        }

        return Task.CompletedTask;
    }
}