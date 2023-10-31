using System.Collections.Concurrent;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IEventsProvider<TEvent>
{
    Task FillWithQueueWithEvents(
        ConcurrentQueue<EventContext<TEvent>> events,
        CancellationToken cancellationToken);
}