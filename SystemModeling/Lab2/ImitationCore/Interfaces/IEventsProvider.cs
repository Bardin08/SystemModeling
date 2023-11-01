using System.Threading.Channels;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IEventsProvider<TEvent>
{
    Task FillWithQueueWithEvents(
        ChannelWriter<EventContext<TEvent>> events,
        CancellationToken cancellationToken);
}