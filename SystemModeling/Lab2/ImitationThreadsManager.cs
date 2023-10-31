using System.Collections.Concurrent;
using System.Threading.Channels;
using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.ImitationCore.Processors;
using SystemModeling.Lab2.Models;
using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Services;

namespace SystemModeling.Lab2;

internal class ImitationThreadsManager<TEvent>
{
    private readonly List<Task> _tasksToRun;
    private readonly IEventsRoutingService<TEvent> _router;
    private readonly IEventsProvider<TEvent> _eventsProvider;
    private readonly ConcurrentQueue<EventContext<TEvent>> _eventStore;
    private readonly IImitationProcessorFactory<TEvent> _imitationProcessorFactory;
    private readonly CancellationToken _cancellationToken;

    public ImitationThreadsManager(
        IRoutingMapService routingMapService,
        IEventsProvider<TEvent> eventsProvider,
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        CancellationToken cancellationToken)
    {
        _eventStore = eventStore;
        _cancellationToken = cancellationToken;
        _eventsProvider = eventsProvider;

        _imitationProcessorFactory = new MultipleConsumersImitationProcessorFactory<TEvent>();
        _router = new EventsRoutingService<TEvent>(eventStore, routingMapService);
        _tasksToRun = new List<Task>();
    }

    public Task RunAllAsync()
    {
        // order matters! Provider -> Router
        var eventsProvider = _eventsProvider.FillWithQueueWithEvents(
            _eventStore, _cancellationToken);
        var routeMessagesTask = _router.RouteAsync(_cancellationToken);

        _tasksToRun.AddRange(new[] { eventsProvider, routeMessagesTask });
        return Task.WhenAll(_tasksToRun);
    }

    public Guid AddImitationProcessor(object options)
    {
        var imitationThreadResult = CreateImitationThread(
            options, _cancellationToken);

        _router.AddRoute(imitationThreadResult.ThreadId.ToString(),
            imitationThreadResult.ChannelWriter);

        _tasksToRun.Add(imitationThreadResult.ThreadExecutable);

        return imitationThreadResult.ThreadId;
    }

    private CreateImitationThreadResult<TEvent> CreateImitationThread(
        object options, CancellationToken ct)
    {
        var channel = Channel.CreateUnbounded<EventContext<TEvent>>();

        var (threadId, task) = _imitationProcessorFactory.GetProcessingTask(
            options, channel.Reader, _eventStore, ct);

        return new CreateImitationThreadResult<TEvent>
        {
            ThreadId = threadId,
            ChannelWriter = channel.Writer,
            ThreadExecutable = task
        };
    }
}