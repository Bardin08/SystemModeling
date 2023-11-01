using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.ImitationCore.Processors;
using SystemModeling.Lab2.Models;
using SystemModeling.Lab2.Routing;
using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Services;

namespace SystemModeling.Lab2;

internal class TasksManager<TEvent>
{
    private readonly List<Task> _tasksToRun;
    private readonly IEventsRoutingService<TEvent> _router;
    private readonly IEventsProvider<TEvent> _eventsProvider;
    private readonly ChannelWriter<EventContext<TEvent>> _eventStoreWriter;
    private readonly IImitationProcessorFactory<TEvent> _imitationProcessorFactory;
    private readonly CancellationToken _cancellationToken;

    public TasksManager(
        IRoutingMapService routingMapService,
        IEventsProvider<TEvent> eventsProvider,
        Channel<EventContext<TEvent>> eventStoreChannel,
        CancellationToken cancellationToken)
    {
        _eventStoreWriter = eventStoreChannel.Writer;
        _cancellationToken = cancellationToken;
        _eventsProvider = eventsProvider;

        _imitationProcessorFactory = new MultipleConsumersImitationProcessorFactory<TEvent>();
        _router = new EventsRoutingService<TEvent>(eventStoreChannel.Reader, routingMapService);
        _tasksToRun = new List<Task>();
    }

    public Task RunAllAsync()
    {
        // order matters! Provider -> Router
        var eventsProvider = _eventsProvider.FillWithQueueWithEvents(
            _eventStoreWriter, _cancellationToken);
        var routeMessagesTask = _router.RouteAsync(_cancellationToken);

        _tasksToRun.AddRange(new[] { eventsProvider, routeMessagesTask });
        return Task.WhenAll(_tasksToRun);
    }

    public Guid AddImitationProcessor(object options, ProcessorNode processorNode)
    {
        var imitationThreadResult = CreateImitationThread(
            options, processorNode, _cancellationToken);

        _router.AddRoute(imitationThreadResult.ThreadId.ToString(),
            imitationThreadResult.ChannelWriter);

        _tasksToRun.Add(imitationThreadResult.ThreadExecutable);

        return imitationThreadResult.ThreadId;
    }

    private CreateImitationThreadResult<TEvent> CreateImitationThread(
        object options, ProcessorNode routingNode, CancellationToken ct)
    {
        var channel = routingNode switch
        {
            { MaxQueueLength: -1 } => Channel.CreateUnbounded<EventContext<TEvent>>(),
            { MaxQueueLength: > 0 } => Channel.CreateBounded<EventContext<TEvent>>(routingNode.MaxQueueLength),
            _ => throw new ArgumentOutOfRangeException(nameof(routingNode), routingNode, null)
        };

        var routingContext = new RoutingContext<TEvent>
        {
            ProcessorOptions = options,
            ProcessorQueue = channel.Reader,
            EventsSource = _eventStoreWriter
        };

        var (threadId, task) = _imitationProcessorFactory
            .GetProcessingTask(routingContext, ct);

        return new CreateImitationThreadResult<TEvent>
        {
            ThreadId = threadId,
            ChannelWriter = channel.Writer,
            ThreadExecutable = task
        };
    }
}