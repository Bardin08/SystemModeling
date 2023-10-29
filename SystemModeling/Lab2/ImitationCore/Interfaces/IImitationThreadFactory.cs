﻿using System.Collections.Concurrent;
using System.Threading.Channels;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IImitationThreadFactory<TEvent>
{
    Task GetProcessingTask(
        ImitationThreadOptions options,
        ChannelReader<EventContext<TEvent>> eventsQueue,
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        CancellationToken ct);
}