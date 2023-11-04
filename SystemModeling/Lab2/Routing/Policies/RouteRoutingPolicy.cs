﻿using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Routing.Policies;

internal class RouteRoutingPolicy<TEvent> : BaseRoutingPolicy<TEvent>
{
    private readonly IRoutingMapService _routingMapService;

    public RouteRoutingPolicy(
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
                if (!processor.TryWrite(eventCtx))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Event wasn't passed to the processor. {0}", eventCtx.Event);
                    Console.ResetColor();
                }
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
                if (!(cumulative >= randomNumber)) continue;

                eventCtx.NextProcessorName = transition.ProcessorName;
                break;
            }
        }

        return Task.CompletedTask;
    }
}