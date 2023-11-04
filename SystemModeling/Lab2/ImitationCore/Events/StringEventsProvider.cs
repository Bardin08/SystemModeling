using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Events;

internal class StringEventsProvider : IEventsProvider<string>
{
    private readonly EventProviderOptions? _eventProviderOptions;

    public StringEventsProvider(EventProviderOptions? eventProviderOptions)
    {
        _eventProviderOptions = eventProviderOptions;
    }

    public Task FillWithQueueWithEvents(
        ChannelWriter<EventContext<string>> events,
        CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            foreach (var i in Enumerable.Range(0, _eventProviderOptions.EventsAmount))
            {
                await events.WriteAsync(
                    new EventContext<string>
                    {
                        EventId = i.ToString(),
                        NextProcessorName = "processor_1",
                        Event = $"Event generated at the {i} iteration"
                    }, cancellationToken);
                await Task.Delay(_eventProviderOptions.AddDelay, cancellationToken);
            }
        }, cancellationToken);
    }
}