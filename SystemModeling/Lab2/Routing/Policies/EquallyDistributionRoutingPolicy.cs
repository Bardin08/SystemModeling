using System.Collections.Concurrent;
using System.Threading.Channels;

namespace SystemModeling.Lab2.Routing.Policies;

internal class EquallyDistributionRoutingPolicy<TEvent> : BaseRoutingPolicy<TEvent>
{
    public EquallyDistributionRoutingPolicy(
        ConcurrentQueue<TEvent> eventsStore,
        ConcurrentDictionary<string, ChannelWriter<TEvent>> handlers)
        : base(eventsStore, handlers)
    {
    }

    public override async Task RouteAsync(object? parameters, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            if (EventsStore.IsEmpty)
            {
                continue;
            }

            foreach (var cw in Handlers.Values)
            {
                if (!EventsStore.TryDequeue(out var @event))
                {
                    continue;
                }

                await cw.WriteAsync(@event, CancellationToken.None);
            }
        }
    }
}