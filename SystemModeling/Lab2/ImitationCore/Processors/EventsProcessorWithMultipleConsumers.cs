using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Processors;

internal class EventsProcessorWithMultipleConsumers<TEvent> : ProcessorBase<TEvent>
{
    public ProcessorWithMultipleConsumersOptions ProcessorOptions { get; }

    public EventsProcessorWithMultipleConsumers(
        ChannelWriter<EventContext<TEvent>> routerQueue,
        ChannelReader<EventContext<TEvent>> processorQueue,
        object processorOptions,
        CancellationTokenSource cancellationTokenSource)
        : base(routerQueue, processorQueue, cancellationTokenSource)
    {
        ProcessorOptions = (processorOptions as ProcessorWithMultipleConsumersOptions)!;
    }

    public override Task ProcessAsync(CancellationToken cancellationToken)
    {
        if (ProcessorOptions.ConsumersAmount < 0 ||
            ProcessorOptions.ProcessorOptions.Count != ProcessorOptions.ConsumersAmount)
            throw new ArgumentException("Consumers amount != configured consumers");

        var task = Task.Run(async () =>
        {
            var tasks = new List<Task>();
            foreach (var procOptions in ProcessorOptions.ProcessorOptions)
            {
                procOptions.ThreadId = ProcessorId;

                tasks.Add(GetProcessingTaskInternal(
                    procOptions, CancellationTokenSource.Token));
            }

            await Task.WhenAll(tasks);
        }, CancellationTokenSource.Token);

        return task;
    }

    private Task GetProcessingTaskInternal(
        ImitationProcessorOptions options, CancellationToken ct)
    {
        return Task.Run(async () =>
        {
            var sb = new StringBuilder();
            while (!ct.IsCancellationRequested)
            {
                sb.Clear();

                if (ProcessorQueue.TryRead(out var @event))
                {
                    ProcessingTime = options.ProcessingTime;

                    const string format = "{0} ({1}): Event: {2}";
                    sb.AppendFormat(format, ProcessorId, options.Alias,
                        JsonConvert.SerializeObject(@event));
                    await RouterQueue.WriteAsync(@event, ct);

                    if (sb.Length > 0)
                    {
                        Console.ForegroundColor = options.Color;
                        Console.WriteLine(sb.ToString());
                        Console.ResetColor();
                    }

                    Notify();
                }

                await Task.Delay(options.ProcessingTime, CancellationToken.None);
            }
        }, ct);
    }
}