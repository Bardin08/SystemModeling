using System.Collections.Concurrent;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Services;

namespace SystemModeling.Lab2;

internal sealed class SimulationProcessor
{
    private readonly SimulationOptions _options;
    private readonly ConcurrentQueue<EventContext<string>> _eventsStore;
    private readonly ImitationThreadsManager<string> _threadsManager;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public SimulationProcessor(SimulationOptions options)
    {
        _options = options;

        _eventsStore = new ConcurrentQueue<EventContext<string>>();
        _cancellationTokenSource = new CancellationTokenSource();

        _threadsManager = new ImitationThreadsManager<string>(
            new RoutingMapService(_options.RoutingMap!),
            _eventsStore,
            _cancellationTokenSource.Token);
    }

    public async Task RunImitationAsync(ImitationOptions options)
    {
        _cancellationTokenSource.CancelAfter(options.ImitationTime);

        // TODO: Add options for events generation thread
        FillWithQueueWithEvents(_eventsStore);

        // TODO: Re-Write threads init logic

        await _threadsManager.RunAllAsync();
        await Task.CompletedTask;
    }

    private void FillWithQueueWithEvents(ConcurrentQueue<EventContext<string>> eventStore)
    {
        foreach (var i in Enumerable.Range(0, 100))
        {
            eventStore.Enqueue(
                new EventContext<string>
                {
                    EventId = i.ToString(),
                    NextProcessorName = "processor_1",
                    Event = $"Event generated at the {i} iteration"
                });
        }
    }
}