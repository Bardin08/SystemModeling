using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;
using Newtonsoft.Json;
using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Threads;

internal class ImitationProcessorFactory<TEvent> : IImitationThreadFactory<TEvent>
{
    public (Guid ThreadId, Task Task) GetProcessingTask(
        object options,
        ChannelReader<EventContext<TEvent>> eventsQueue,
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        CancellationToken ct)
    {
        var threadId = Guid.NewGuid();
        
        var threadOptions = (options as ImitationProcessorOptions)!;
        threadOptions.ThreadId = threadId;

        var task = Task.Run(async () =>
        {
            var sb = new StringBuilder();
            while (!ct.IsCancellationRequested)
            {
                sb.Clear();

                if (eventsQueue.TryRead(out var @event))
                {
                    sb.Append($"{threadOptions.ThreadId}: Event: {JsonConvert.SerializeObject(@event)}");
                    eventStore.Enqueue(@event);
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