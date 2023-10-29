using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;
using Newtonsoft.Json;
using SystemModeling.Lab2.Models;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Services;

namespace SystemModeling.Lab2;

internal class ImitationThreadsManager<TEvent>
{
    private readonly IEventsRoutingService<TEvent> _router;
    private readonly CancellationToken _cancellationToken;
    private readonly ConcurrentQueue<EventContext<TEvent>> _eventStore;
    private readonly List<Task> _threads;

    public ImitationThreadsManager(
        IRoutingMapService routingMapService,
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        CancellationToken cancellationToken)
    {
        _eventStore = eventStore;
        _cancellationToken = cancellationToken;

        _router = new EventsRoutingService<TEvent>(eventStore, routingMapService);
        _threads = new List<Task>();
    }

    public async Task RunAllAsync()
    {
        var routeMessagesTask = Task
            .Run(() => _router.RouteAsync(_cancellationToken), _cancellationToken);

        _threads.AddRange(new[] { routeMessagesTask });
        await Task.WhenAll(_threads);
    }

    public Guid AddImitationThread(TimeSpan processingTime)
    {
        var imitationThreadResult = CreateImitationThread(
            processingTime, _cancellationToken);

        _router.AddRoute(imitationThreadResult.ThreadId.ToString(),
            imitationThreadResult.ChannelWriter);

        _threads.Add(imitationThreadResult.ThreadExecutable);

        return imitationThreadResult.ThreadId;
    }

    private CreateImitationThreadResult<TEvent> CreateImitationThread(
        TimeSpan processingTime, CancellationToken ct)
    {
        var channel = Channel.CreateUnbounded<EventContext<TEvent>>();

        var threadId = Guid.NewGuid();
        var options = new ImitationThreadOptions()
        {
            ThreadId = threadId,
            ProcessingTime = processingTime
        };

        var thread = GetImitationThreadInternalAsync(channel.Reader, options, _eventStore, ct);

        return new CreateImitationThreadResult<TEvent>()
        {
            ThreadId = threadId,
            ChannelWriter = channel.Writer,
            ThreadExecutable = thread
        };
    }

    private async Task GetImitationThreadInternalAsync(
        ChannelReader<EventContext<TEvent>> eventsQueue,
        ImitationThreadOptions options,
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        CancellationToken ct)
    {
        var sb = new StringBuilder();
        while (!ct.IsCancellationRequested)
        {
            sb.Clear();

            if (eventsQueue.TryRead(out var @event))
            {
                sb.Append($"{options.ThreadId}: Event: {JsonConvert.SerializeObject(@event)}");
                eventStore.Enqueue(@event);
            }

            if (sb.Length > 0)
            {
                Console.WriteLine(sb.ToString());
            }

            await Task.Delay(options.ProcessingTime, CancellationToken.None);
        }
    }
}