﻿using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Models;

internal record CreateImitationThreadResult<TEvent>
{
    public required Guid ThreadId { get; init; }
    public required ChannelWriter<EventContext<TEvent>> ChannelWriter { get; init; }
    public required Task ThreadExecutable { get; init; }
}