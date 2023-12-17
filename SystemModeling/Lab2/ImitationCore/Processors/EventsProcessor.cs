using SystemModeling.Lab2.ImitationCore.Backoffs;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Processors;

internal class EventsProcessor<TEvent> : ProcessorBase<TEvent>
{
    private ProcessorWithMultipleConsumersOptions ProcessorOptions { get; }

    public EventsProcessor(
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
                if (ProcessorQueue.Completion.IsCompleted) break;

                sb.Clear();

                if (ProcessorQueue.TryRead(out var @event))
                {
                    var processing = options.ProcessingTimeProvider?.GetBackoff() ?? ProcessingTime;
                    ProcessingTime = processing;

                    await Task.Delay(processing, CancellationToken.None);

                    try
                    {
                        await RouterQueue.WriteAsync(@event, ct);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }

                    Notify();
                }
            }
        }, ct);
    }
}