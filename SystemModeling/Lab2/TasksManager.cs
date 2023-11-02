using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.ImitationCore.Observers;
using SystemModeling.Lab2.ImitationCore.Processors;
using SystemModeling.Lab2.Models;
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
    private readonly CancellationTokenSource _cancellationTokenSource;

    public TasksManager(
        IRoutingMapService routingMapService,
        IEventsProvider<TEvent> eventsProvider,
        Channel<EventContext<TEvent>> eventStoreChannel,
        CancellationTokenSource cancellationTokenSource)
    {
        _eventStoreWriter = eventStoreChannel.Writer;
        _cancellationTokenSource = cancellationTokenSource;
        _eventsProvider = eventsProvider;

        _router = new EventsRoutingService<TEvent>(eventStoreChannel.Reader, routingMapService);
        _tasksToRun = new List<Task>();
    }

    public Task RunAllAsync()
    {
        // order matters! Provider -> Router
        var eventsProvider = _eventsProvider.FillWithQueueWithEvents(
            _eventStoreWriter, _cancellationTokenSource.Token);
        var routeMessagesTask = _router.RouteAsync(_cancellationTokenSource.Token);

        _tasksToRun.AddRange(new[] { eventsProvider, routeMessagesTask });
        return Task.WhenAll(_tasksToRun);
    }

    public Guid AddImitationProcessor(object options, ProcessorNode processorNode)
    {
        var imitationThreadResult = CreateImitationThread(options, processorNode);

        _router.AddRoute(imitationThreadResult.ThreadId.ToString(),
            imitationThreadResult.ChannelWriter);

        _tasksToRun.Add(imitationThreadResult.ThreadExecutable);

        return imitationThreadResult.ThreadId;
    }

    private CreateImitationThreadResult<TEvent> CreateImitationThread(
        object options, ProcessorNode routingNode)
    {
        var channel = routingNode switch
        {
            { MaxQueueLength: -1 } => Channel.CreateUnbounded<EventContext<TEvent>>(),
            { MaxQueueLength: > 0 } => Channel.CreateBounded<EventContext<TEvent>>(routingNode.MaxQueueLength),
            _ => throw new ArgumentOutOfRangeException(nameof(routingNode), routingNode, null)
        };

        var imitationProcessor = new EventsProcessorWithMultipleConsumers<TEvent>(
            _eventStoreWriter,
            channel.Reader,
            options,
            _cancellationTokenSource);
        imitationProcessor.RegisterHandler(new ProcessorEventConsumptionObserver());


        var task = imitationProcessor.ProcessAsync(_cancellationTokenSource.Token);

        return new CreateImitationThreadResult<TEvent>
        {
            ThreadId = imitationProcessor.ProcessorId,
            ChannelWriter = channel.Writer,
            ThreadExecutable = task
        };
    }
}