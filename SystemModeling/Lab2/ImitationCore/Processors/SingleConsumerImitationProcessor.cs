using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing;

namespace SystemModeling.Lab2.ImitationCore.Processors;

internal class SingleConsumerImitationProcessor<TEvent>
    : IImitationProcessor<TEvent>
{
    public (Guid ThreadId, Task Task) GetProcessingTask(
        RoutingContext<TEvent> routingContext, 
        CancellationToken ct)
    {
        var threadId = Guid.NewGuid();

        var threadOptions = (routingContext.ProcessorOptions as ImitationProcessorOptions)!;
        threadOptions.ThreadId = threadId;

        var task = Task.Run(async () =>
        {
            var sb = new StringBuilder();
            while (!ct.IsCancellationRequested)
            {
                sb.Clear();

                if (routingContext.ProcessorQueue.TryRead(out var @event))
                {
                    sb.Append($"{threadOptions.ThreadId}: Event: {JsonConvert.SerializeObject(@event)}");
                    await routingContext.EventsSource.WriteAsync(@event, ct);
                }

                if (sb.Length > 0)
                {
                    Console.WriteLine(sb.ToString());
                }

                await Task.Delay(threadOptions.ProcessingTime, CancellationToken.None);
            }
        }, ct);

        return (threadId, task);
    }
}