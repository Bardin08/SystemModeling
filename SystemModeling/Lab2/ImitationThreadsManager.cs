using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;
using SystemModeling.Lab2.Configuration;
using SystemModeling.Lab2.Models;

namespace SystemModeling.Lab2;

internal class ImitationThreadsManager
{
    private readonly EventRouter<string> _router;
    private readonly CancellationToken _cancellationToken;
    private readonly List<Task> _threads;

    public ImitationThreadsManager(
        ConcurrentQueue<string> eventStore,
        CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;

        _router = new EventRouter<string>(eventStore);
        _threads = new List<Task>();
    }

    public async Task RunAllAsync()
    {
        var routeMessagesTask = _router.RouteAsync(_cancellationToken);

        _threads.AddRange(new[] { routeMessagesTask });
        await Task.WhenAll(_threads);
    }

    public void AddImitationThread(TimeSpan processingTime)
    {
        var imitationThreadResult = CreateImitationThread<string>(
            processingTime, _cancellationToken);

        _router.AddRoute(imitationThreadResult.ThreadId.ToString(),
            imitationThreadResult.ChannelWriter);

        _threads.Add(imitationThreadResult.ThreadExecutable);
    }



    private CreateImitationThreadResult<TEvent> CreateImitationThread<TEvent>(
        TimeSpan processingTime, CancellationToken ct)
    {
        var channel = Channel.CreateUnbounded<TEvent>();

        var threadId = Guid.NewGuid();
        var options = new ImitationThreadOptions()
        {
            ThreadId = threadId,
            ProcessingTime = processingTime
        };

        var thread = GetImitationThreadInternalAsync(channel.Reader, options, ct);

        return new CreateImitationThreadResult<TEvent>()
        {
            ThreadId = threadId,
            ChannelWriter = channel.Writer,
            ThreadExecutable = thread
        };
    }

    private async Task GetImitationThreadInternalAsync<TEvent>(
        ChannelReader<TEvent> eventsQueue,
        ImitationThreadOptions options,
        CancellationToken ct)
    {
        var sb = new StringBuilder();
        while (!ct.IsCancellationRequested)
        {
            sb.Clear();
            sb.Append($"[{DateTime.Now:hh:mm:ss}] {options.ThreadId.ToString()[..6]}: Iteration started...")
                .AppendLine();

            if (eventsQueue.Count is 0)
            {
                sb.Append("\tEvents queue is empty").AppendLine();
                sb.Append($"[{DateTime.Now:hh:mm:ss}] {options.ThreadId.ToString()[..6]}: Iteration ended...")
                    .AppendLine();
                Console.WriteLine(sb.ToString());
                await Task.Delay(options.ProcessingTime, CancellationToken.None);
                continue;
            }

            if (eventsQueue.TryRead(out var @event))
            {
                sb.Append("\tEvent received.").AppendLine();
                sb.Append($"\tEvent: {@event}").AppendLine();
            }

            sb.Append($"[{DateTime.Now:hh:mm:ss}] {options.ThreadId.ToString()[..6]}: Iteration ended...");
            Console.WriteLine(sb.ToString());
            await Task.Delay(options.ProcessingTime, CancellationToken.None);
        }
    }
}