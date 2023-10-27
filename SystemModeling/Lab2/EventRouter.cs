using System.Collections.Concurrent;
using System.Threading.Channels;

namespace SystemModeling.Lab2;

public class EventRouter<TEvent>
{
    private readonly ConcurrentQueue<TEvent> _eventStore;
    private readonly ConcurrentDictionary<string, ChannelWriter<TEvent>> _handlers;

    public EventRouter(ConcurrentQueue<TEvent> eventStore)
    {
        _eventStore = eventStore;

        _handlers = new ConcurrentDictionary<string, ChannelWriter<TEvent>>();
    }

    public bool AddRoute(string routeId, ChannelWriter<TEvent> channelWriter)
    {
        return _handlers.TryAdd(routeId, channelWriter);
    }

    public bool RemoveRoute(string routeId)
    {
        if (!_handlers.TryGetValue(routeId, out var channelWriter))
        {
            return false;
        }

        channelWriter.Complete();
        return _handlers.TryRemove(routeId, out _);
    }

    public async Task RouteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            if (_eventStore.IsEmpty)
            {
                continue;
            }

            foreach (var cw in _handlers.Values)
            {
                if (!_eventStore.TryDequeue(out var @event))
                {
                    continue;
                }

                await cw.WriteAsync(@event, CancellationToken.None);
            }
        }
    }
}