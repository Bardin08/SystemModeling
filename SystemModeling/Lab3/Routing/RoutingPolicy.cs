using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Policies;
using SystemModeling.Lab3.Extensions;

namespace SystemModeling.Lab3.Routing;

internal class RoutingPolicy(
    IRoutingMapService routingMap,
    ChannelReader<EventContext<int>> eventsStore,
    ConcurrentDictionary<string, Channel<EventContext<int>>> handlers)
    : BaseRoutingPolicy<int>(eventsStore, handlers)
{
    public override async Task RouteAsync(object? parameters, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await WriteEventToQueueAsync(ct);
            // probably I have to use some locking object here
            // to avoid queues modification in any other thread
            await BalanceQueuesAsync(ct);
        }
    }

    private async Task BalanceQueuesAsync(CancellationToken ct)
    {
        var accessors = GetQueuesAccessors();
        var (q1, q2) = (accessors.Item1, accessors.Item2);

        var q1Size = q1.Reader.Count;
        var q2Size = q2.Reader.Count;

        //  len(q1) - len(q2) == 2 -> swap(q1, q2)
        await (q1Size.CompareTo(q2Size) switch
        {
            2 => q1.TransferLastItem(q2.Writer, ct),
            -2 => q2.TransferLastItem(q1.Writer, ct),
            _ => Task.CompletedTask
        });
    }

    private async Task WriteEventToQueueAsync(CancellationToken ct)
    {
        await Task.Yield();

        var eventCtx = await GetEventContextAsync(ct);
        if (eventCtx is null)
        {
            return;
        }

        var queueWriter = GetQueueWriter();
        await queueWriter.WriteAsync(eventCtx, ct);
    }

    private async Task<EventContext<int>?> GetEventContextAsync(CancellationToken ct)
    {
        // if there are 0 elems in the router queue take a brake 100ms and continue
        if (EventsStore.Count is 0)
        {
            await Task.Yield();
            await Task.Delay(100, ct);
        }

        if (!EventsStore.TryRead(out var eventCtx))
        {
            return null;
        }

        // no need to route it if next processor is 'complete'
        return eventCtx.NextProcessorName is "complete" ? null : eventCtx;
    }

    private ChannelWriter<EventContext<int>> GetQueueWriter()
    {
        var accessors = GetQueuesAccessors();
        var (q1, q2) = (accessors.Item1, accessors.Item2);

        var q1Size = q1.Reader.Count;
        var q2Size = q2.Reader.Count;

        // len(q1) == len(q2) -> q1
        // min(len(q1, q2)) -> q(1|2)
        return q1Size.CompareTo(q2Size) switch
        {
            > 0 => q1.Writer,
            < 0 => q2.Writer,
            _ => q1.Writer
        };
    }

    private (Channel<EventContext<int>>, Channel<EventContext<int>>) GetQueuesAccessors()
    {
        var processors = GetProcessorNode();
        ArgumentNullException.ThrowIfNull(processors);

        var (q1Node, q2Node) = (processors.Value.q1, processors.Value.q2);

        var queue1 = Handlers!.GetValueOrDefault(q1Node.RouteId);
        var queue2 = Handlers!.GetValueOrDefault(q2Node.RouteId);

        ArgumentNullException.ThrowIfNull(queue1);
        ArgumentNullException.ThrowIfNull(queue2);

        return (queue1, queue2);
    }

    private (ProcessorNode q1, ProcessorNode q2)? GetProcessorNode()
    {
        var queue1Node = routingMap.GetProcessorNodeByName("queue1");
        var queue2Node = routingMap.GetProcessorNodeByName("queue2");

        return (queue1Node, queue2Node)!;
    }
}