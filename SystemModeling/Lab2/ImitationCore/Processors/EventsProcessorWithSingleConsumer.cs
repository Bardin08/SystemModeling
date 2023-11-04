using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Processors;

internal class EventsProcessorWithSingleConsumer<TEvent>
    : ProcessorBase<TEvent>
{
    private ImitationProcessorOptions ProcessorOptions { get; }

    public EventsProcessorWithSingleConsumer(
        ChannelWriter<EventContext<TEvent>> routerQueue,
        ChannelReader<EventContext<TEvent>> processorQueue,
        ImitationProcessorOptions processorOptions,
        CancellationTokenSource cancellationTokenSource)
        : base(routerQueue, processorQueue, cancellationTokenSource)
    {
        ProcessorOptions = processorOptions;
    }

    public override Task ProcessAsync(CancellationToken cancellationToken)
    {
        ProcessorOptions.ThreadId = ProcessorId;

        var task = Task.Run(async () =>
        {
            var sb = new StringBuilder();
            while (!CancellationTokenSource.Token.IsCancellationRequested)
            {
                sb.Clear();

                if (ProcessorQueue.TryRead(out var @event))
                {
                    sb.Append($"{ProcessorId}: Event: {JsonConvert.SerializeObject(@event)}");
                    await RouterQueue.WriteAsync(@event, CancellationTokenSource.Token);
                }

                if (sb.Length > 0)
                {
                    Console.WriteLine(sb.ToString());
                }

                await Task.Delay(ProcessorOptions.ProcessingTime, CancellationTokenSource.Token);
            }
        }, CancellationTokenSource.Token);

        return task;
    }
}