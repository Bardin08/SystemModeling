using SystemModeling.Lab2.Configuration;

namespace SystemModeling.Lab2;

internal sealed class ImitationProcessor
{
    private readonly ImitationProcessorOptions _options;

    public ImitationProcessor(ImitationProcessorOptions options)
    {
        _options = options;
    }

    public async Task RunImitationAsync(ImitationOptions options)
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(options.ImitationTime);

        var imitationTask1 = GetImitationTask(TimeSpan.FromSeconds(5), cts);
        var imitationTask2 = GetImitationTask(TimeSpan.FromSeconds(2), cts);

        await Task.WhenAll(imitationTask1, imitationTask2);

        await Task.CompletedTask;
    }

    private Task GetImitationTask(TimeSpan sleepTime, CancellationTokenSource cts)
    {
        var threadOptions = new ImitationThreadOptions
        {
            ThreadId = Guid.NewGuid(),
            SleepTime = sleepTime
        };

        return GetImitationThreadInternalAsync(threadOptions, cts.Token);
    }

    private async Task GetImitationThreadInternalAsync(
        ImitationThreadOptions options, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] {options.ThreadId.ToString()[..6]}: Imitation running...");
            await Task.Delay(options.SleepTime, CancellationToken.None);
        }
    }
}