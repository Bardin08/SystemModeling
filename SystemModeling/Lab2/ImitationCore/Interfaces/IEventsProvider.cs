using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IEventsProvider<TEvent>
{
    Task FillWithQueueWithEvents(
        ChannelReader<EventContext<TEvent>> reader,
        ChannelWriter<EventContext<TEvent>> events,
        CancellationToken cancellationToken);
}