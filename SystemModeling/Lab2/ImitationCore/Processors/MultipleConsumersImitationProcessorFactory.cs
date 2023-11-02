using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing;

namespace SystemModeling.Lab2.ImitationCore.Processors;

internal class MultipleConsumersImitationProcessorFactory<TEvent>
    : IImitationProcessorFactory<TEvent>
{
    public (Guid ThreadId, Task Task) GetProcessingTask(
        RoutingContext<TEvent> routingContext, CancellationToken ct)
    {
        var processorOptions = (routingContext.ProcessorOptions as MultiConsumersImitationProcessorOptions)!;
        if (processorOptions.ConsumersAmount < 0 ||
            processorOptions.ProcessorOptions.Count != processorOptions.ConsumersAmount)
        {
            return (Guid.Empty, Task.CompletedTask);
        }

        var threadId = Guid.NewGuid();
        var task = Task.Run(async () =>
        {
            var tasks = new List<Task>();
            foreach (var procOptions in processorOptions.ProcessorOptions)
            {
                procOptions.ThreadId = threadId;
                var localRoutingContext = new RoutingContext<TEvent>
                {
                    ProcessorOptions = procOptions,
                    EventsSource = routingContext.EventsSource,
                    ProcessorQueue = routingContext.ProcessorQueue
                };

                tasks.Add(GetProcessingTaskInternal(localRoutingContext, ct));
            }

            await Task.WhenAll(tasks);
        }, ct);
        return (threadId, task);
    }

    private Task GetProcessingTaskInternal(
        RoutingContext<TEvent> routingContext, CancellationToken ct)
    {
        var processorOptions = (routingContext.ProcessorOptions as ImitationProcessorOptions)!;
        return Task.Run(async () =>
        {
            var sb = new StringBuilder();
            while (!ct.IsCancellationRequested)
            {
                sb.Clear();

                if (routingContext.ProcessorQueue.TryRead(out var @event))
                {
                    sb.Append(
                        $"{processorOptions.ThreadId} ({processorOptions.Alias}): Event: {JsonConvert.SerializeObject(@event)}");
                    await routingContext.EventsSource.WriteAsync(@event, ct);
                }

                if (sb.Length > 0)
                {
                    Console.ForegroundColor = processorOptions.Color;
                    Console.WriteLine(sb.ToString());
                    Console.ResetColor();
                }

                await Task.Delay(processorOptions.ProcessingTime, CancellationToken.None);
            }
        }, ct);
    }
}