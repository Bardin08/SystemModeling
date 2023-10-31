using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;
using Newtonsoft.Json;
using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Threads;

internal class MultiConsumersImitationProcessorFactory<TEvent>
    : IImitationThreadFactory<TEvent>
{
    public (Guid ThreadId, Task Task) GetProcessingTask(
        object options,
        ChannelReader<EventContext<TEvent>> eventsQueue,
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        CancellationToken ct)
    {
        var processorOptions = (options as MultiConsumersImitationProcessorOptions)!;
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
                tasks.Add(GetProcessingTaskInternal(
                    procOptions, eventsQueue, eventStore, ct));
            }

            await Task.WhenAll(tasks);
        }, ct);
        return (threadId, task);
    }

    private Task GetProcessingTaskInternal(
        ImitationProcessorOptions options,
        ChannelReader<EventContext<TEvent>> eventsQueue,
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        CancellationToken ct)
    {
        return Task.Run(async () =>
        {
            var sb = new StringBuilder();
            var lockObj = new object();
            while (!ct.IsCancellationRequested)
            {
                lock (lockObj)
                {
                    sb.Clear();

                    if (eventsQueue.TryRead(out var @event))
                    {
                        sb.Append($"{options.ThreadId} ({options.Alias}): Event: {JsonConvert.SerializeObject(@event)}");
                        eventStore.Enqueue(@event);
                    }

                    if (sb.Length > 0)
                    {
                        Console.ForegroundColor = options.Color;
                        Console.WriteLine(sb.ToString());
                        Console.ResetColor();
                    }
                }

                await Task.Delay(options.ProcessingTime, CancellationToken.None);
            }
        }, ct);
    }
}