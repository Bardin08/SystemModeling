using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.ImitationCore.Models;
using SystemModeling.Lab2.ImitationCore.Observers;
using SystemModeling.Lab2.ImitationCore.Processors;
using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Services;

namespace SystemModeling.Lab2;

internal class TasksManager<TEvent>
{
    private readonly List<Task> _tasksToRun;
    private readonly IEventsRoutingService<TEvent> _router;
    private readonly IEventsProvider<TEvent> _eventsProvider;
    private readonly Channel<EventContext<TEvent>, EventContext<TEvent>> _eventsChannel;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public TasksManager(
        IRoutingMapService routingMapService,
        IEventsProvider<TEvent> eventsProvider,
        Channel<EventContext<TEvent>, EventContext<TEvent>> eventsStoreChannel,
        CancellationTokenSource cancellationTokenSource)
    {
        _eventsChannel = eventsStoreChannel;
        _cancellationTokenSource = cancellationTokenSource;
        _eventsProvider = eventsProvider;

        _router = new EventsRoutingService<TEvent>(eventsStoreChannel.Reader, routingMapService);
        _tasksToRun = new List<Task>();
    }

    public Task RunAllAsync()
    {
        AddInternalThreads();
        return Task.WhenAll(_tasksToRun);
    }

    private void AddInternalThreads()
    {
        // order matters! Provider -> Router
        _tasksToRun.AddRange(new[]
        {
            GetEventsGeneratorThread(),
            GetEventsRoutingThread()
        });
    }

    private Task GetEventsGeneratorThread()
    {
        return _eventsProvider.FillWithQueueWithEvents(
            _eventsChannel.Reader,
            _eventsChannel.Writer,
            _cancellationTokenSource.Token);
    }

    private Task GetEventsRoutingThread()
    {
        return _router.RouteAsync(_cancellationTokenSource.Token);
    }

    public CreateProcessorResultDto<TEvent> AddImitationProcessor(
        object options, ProcessorNode processorNode)
    {
        var threadInfo = CreateImitationThread(options, processorNode);
        _router.AddRoute(threadInfo.ThreadId.ToString(), threadInfo.ChannelWriter);
        _tasksToRun.Add(threadInfo.ThreadExecutable);

        return threadInfo;
    }

    private CreateProcessorResultDto<TEvent> CreateImitationThread(
        object options, ProcessorNode routingNode)
    {
        var channel = routingNode switch
        {
            { MaxQueueLength: -1 } => Channel.CreateUnbounded<EventContext<TEvent>>(),
            { MaxQueueLength: > 0 } => Channel.CreateBounded<EventContext<TEvent>>(routingNode.MaxQueueLength),
            _ => throw new ArgumentOutOfRangeException(nameof(routingNode), routingNode, null)
        };

        var imitationProcessor = new EventsProcessor<TEvent>(
            _eventsChannel.Writer, channel.Reader, options, _cancellationTokenSource);

        var statisticsObserver = new EventProcessorStateObserver(imitationProcessor.ProcessorId);
        imitationProcessor.RegisterObserver(statisticsObserver);

        var task = imitationProcessor.ProcessAsync(_cancellationTokenSource.Token);

        return new CreateProcessorResultDto<TEvent>
        {
            ThreadId = imitationProcessor.ProcessorId,
            ChannelWriter = channel.Writer,
            ThreadExecutable = task,
            GetProcessorStatsFunc = () => (statisticsObserver as IEventProcessorStateObserver)
                .GetProcessorStatistics()!,
            RoutingStatsFunc = () => _router.RoutingResultObserver(routingNode.Name!)
        };
    }
}