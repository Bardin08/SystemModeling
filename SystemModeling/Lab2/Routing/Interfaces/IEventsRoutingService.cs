﻿using SystemModeling.Lab2.ImitationCore.Observers;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Routing.Interfaces;

internal interface IEventsRoutingService<TEvent>
{
    ProcessorRoutingDescriptor RoutingResultObserver(string processorName);

    bool AddRoute(string routeId, ChannelWriter<EventContext<TEvent>> channelWriter);

    Task RouteAsync(CancellationToken ct);
}