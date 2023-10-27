using System.Collections.Concurrent;
using SystemModeling.Lab2.Configuration;

namespace SystemModeling.Lab2;

internal sealed class ImitationProcessor
{
    private readonly ImitationProcessorOptions _options;
    private readonly ConcurrentQueue<string> _eventsStore;
    private readonly ImitationThreadsManager _threadsManager;
    private readonly CancellationTokenSource _cancellationTokenSource;
    

    public ImitationProcessor(ImitationProcessorOptions options)
    {
        _options = options;

        _eventsStore = new ConcurrentQueue<string>();
        _cancellationTokenSource = new CancellationTokenSource();

        _threadsManager = new ImitationThreadsManager(
            _eventsStore, _cancellationTokenSource.Token);
    }

    public async Task RunImitationAsync(ImitationOptions options)
    {
        _cancellationTokenSource.CancelAfter(options.ImitationTime);
        FillWithQueueWithEvents(_eventsStore);

        _threadsManager.AddImitationThread(TimeSpan.FromSeconds(2));
        _threadsManager.AddImitationThread(TimeSpan.FromSeconds(3));

        await _threadsManager.RunAllAsync();
        await Task.CompletedTask;
    }

    private void FillWithQueueWithEvents(ConcurrentQueue<string> eventStore)
    {
        foreach (var i in Enumerable.Range(0, 100))
        {
            eventStore.Enqueue($"Event generated at the {i} iteration");
        }
    }
}