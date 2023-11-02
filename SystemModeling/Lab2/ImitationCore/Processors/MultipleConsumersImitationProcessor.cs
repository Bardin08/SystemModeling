using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing;

namespace SystemModeling.Lab2.ImitationCore.Processors;

internal class MultipleConsumersImitationProcessor<TEvent>
    : IImitationProcessor<TEvent>, IObservable
{
    private readonly List<IObserver> _observers = new();

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
                    const string format = "{0} ({1}): Event: {2}";
                    sb.AppendFormat(format, processorOptions.ThreadId, processorOptions.Alias,
                        JsonConvert.SerializeObject(@event));
                    await routingContext.EventsSource.WriteAsync(@event, ct);

                    if (sb.Length > 0)
                    {
                        Console.ForegroundColor = processorOptions.Color;
                        Console.WriteLine(sb.ToString());
                        Console.ResetColor();
                    }

                    Notify();
                }

                await Task.Delay(processorOptions.ProcessingTime, CancellationToken.None);
            }
        }, ct);
    }

    public void RegisterHandler(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveHandler(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        _observers.ForEach(o => o.Handle(this));
    }
}