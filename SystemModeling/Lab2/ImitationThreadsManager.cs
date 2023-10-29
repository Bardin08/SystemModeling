using System.Collections.Concurrent;
using System.Threading.Channels;
using SystemModeling.Lab2.ImitationCore.Threads;
using SystemModeling.Lab2.Models;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Services;

namespace SystemModeling.Lab2;

internal class ImitationThreadsManager<TEvent>
{
    private readonly IEventsRoutingService<TEvent> _router;
    private readonly IImitationThreadFactory<TEvent> _imitationThreadFactory;
    private readonly ConcurrentQueue<EventContext<TEvent>> _eventStore;
    private readonly List<Task> _tasksToRun;
    private readonly CancellationToken _cancellationToken;

    public ImitationThreadsManager(
        IRoutingMapService routingMapService,
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        CancellationToken cancellationToken)
    {
        _eventStore = eventStore;
        _cancellationToken = cancellationToken;

        _imitationThreadFactory = new ImitationProcessorFactory<TEvent>();
        _router = new EventsRoutingService<TEvent>(eventStore, routingMapService);
        _tasksToRun = new List<Task>();
    }

    public async Task RunAllAsync()
    {
        var routeMessagesTask = Task
            .Run(() => _router.RouteAsync(_cancellationToken), _cancellationToken);

        _tasksToRun.AddRange(new[] { routeMessagesTask });
        await Task.WhenAll(_tasksToRun);
    }

    public Guid AddImitationProcessor(TimeSpan processingTime)
    {
        var imitationThreadResult = CreateImitationThread(
            processingTime, _cancellationToken);

        _router.AddRoute(imitationThreadResult.ThreadId.ToString(),
            imitationThreadResult.ChannelWriter);

        _tasksToRun.Add(imitationThreadResult.ThreadExecutable);

        return imitationThreadResult.ThreadId;
    }

    private CreateImitationThreadResult<TEvent> CreateImitationThread(
        TimeSpan processingTime, CancellationToken ct)
    {
        var channel = Channel.CreateUnbounded<EventContext<TEvent>>();

        var threadId = Guid.NewGuid();
        var options = new ImitationThreadOptions
        {
            ThreadId = threadId,
            ProcessingTime = processingTime
        };

        var thread = _imitationThreadFactory.GetProcessingTask(
            options, channel.Reader, _eventStore, ct);

        return new CreateImitationThreadResult<TEvent>
        {
            ThreadId = threadId,
            ChannelWriter = channel.Writer,
            ThreadExecutable = thread
        };
    }
}