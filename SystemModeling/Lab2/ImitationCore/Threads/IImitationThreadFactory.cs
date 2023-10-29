using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;
using Newtonsoft.Json;
using SystemModeling.Lab2.Models;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Threads;

internal interface IImitationThreadFactory<TEvent>
{
    Task GetProcessingTask(
        ImitationThreadOptions options,
        ChannelReader<EventContext<TEvent>> eventsQueue,
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        CancellationToken ct);
}

internal class ImitationProcessorFactory<TEvent> : IImitationThreadFactory<TEvent>
{
    public Task GetProcessingTask(
        ImitationThreadOptions options,
        ChannelReader<EventContext<TEvent>> eventsQueue,
        ConcurrentQueue<EventContext<TEvent>> eventStore,
        CancellationToken ct)
    {
        return Task.Run(async () =>
        {
            var sb = new StringBuilder();
            while (!ct.IsCancellationRequested)
            {
                sb.Clear();

                if (eventsQueue.TryRead(out var @event))
                {
                    sb.Append($"{options.ThreadId}: Event: {JsonConvert.SerializeObject(@event)}");
                    eventStore.Enqueue(@event);
                }

                if (sb.Length > 0)
                {
                    Console.WriteLine(sb.ToString());
                }

                await Task.Delay(options.ProcessingTime, CancellationToken.None);
            }
        }, ct);
    }
}