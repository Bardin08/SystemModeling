using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Events;

internal class StringEventsProvider : IEventsProvider<string>
{
    private readonly EventProviderOptions _eventProviderOptions;

    public StringEventsProvider(EventProviderOptions? eventProviderOptions)
    {
        ArgumentNullException.ThrowIfNull(eventProviderOptions);
        _eventProviderOptions = eventProviderOptions;
    }

    public Task FillWithQueueWithEvents(
        ChannelReader<EventContext<string>> reader,
        ChannelWriter<EventContext<string>> events,
        CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            foreach (var i in Enumerable.Range(0, _eventProviderOptions.EventsAmount))
            {
                try
                {
                    var isWrote = events.TryWrite(
                        new EventContext<string>
                        {
                            EventId = i.ToString(),
                            NextProcessorName = _eventProviderOptions.ProcessorName,
                            Event = $"Event generated at the {i} iteration"
                        });

                    Console.WriteLine("Writing event to queue. Result: {0}", isWrote);
                    Console.WriteLine("Queue size: {0}", reader.Count);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                await Task.Delay(_eventProviderOptions.AddDelay, cancellationToken);
            }
        }, cancellationToken);
    }
}