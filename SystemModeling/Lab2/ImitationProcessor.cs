using System.Collections.Concurrent;
using SystemModeling.Lab2.Configuration;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Services;

namespace SystemModeling.Lab2;

internal sealed class ImitationProcessor
{
    private readonly ImitationProcessorOptions _options;
    private readonly ConcurrentQueue<EventContext<string>> _eventsStore;
    private readonly ImitationThreadsManager<string> _threadsManager;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly List<ProcessorNode> _processorNodes;

    public ImitationProcessor(ImitationProcessorOptions options)
    {
        _options = options;

        _eventsStore = new ConcurrentQueue<EventContext<string>>();
        _cancellationTokenSource = new CancellationTokenSource();
        _processorNodes = new List<ProcessorNode>();

        _threadsManager = new ImitationThreadsManager<string>(
            new RoutingMapService(_processorNodes), _eventsStore, _cancellationTokenSource.Token);
    }

    public async Task RunImitationAsync(ImitationOptions options)
    {
        _cancellationTokenSource.CancelAfter(options.ImitationTime);
        FillWithQueueWithEvents(_eventsStore);

        var threadId1 = _threadsManager.AddImitationThread(TimeSpan.FromSeconds(5));
        var threadId2 = _threadsManager.AddImitationThread(TimeSpan.FromSeconds(5));
        var threadId3 = _threadsManager.AddImitationThread(TimeSpan.FromSeconds(5));

        _processorNodes.Add(new ProcessorNode
        {
            Name = "processor_1",
            RouteId = threadId1.ToString(),
            Transitions = new List<TransitionNode>
            {
                new()
                {
                    Name = "processor_1__processor_2",
                    TransitionChance = 1,
                    ProcessorName = "processor_2"
                }
            }
        });
        
        _processorNodes.Add(new ProcessorNode
        {
            Name = "processor_2",
            RouteId = threadId2.ToString(),
            Transitions = new List<TransitionNode>
            {
                new()
                {
                    Name = "processor_2__processor_3",
                    TransitionChance = 1,
                    ProcessorName = "processor_3"
                }
            }
        });

        _processorNodes.Add(new ProcessorNode
        {
            Name = "processor_3",
            RouteId = threadId3.ToString(),
            Transitions = new List<TransitionNode>
            {
                new()
                {
                    Name = "processor_3__complete",
                    TransitionChance = 1,
                    ProcessorName = "complete"
                }
            }
        });

        await _threadsManager.RunAllAsync();
        await Task.CompletedTask;
    }

    private void FillWithQueueWithEvents(ConcurrentQueue<EventContext<string>> eventStore)
    {
        foreach (var i in Enumerable.Range(0, 1))
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