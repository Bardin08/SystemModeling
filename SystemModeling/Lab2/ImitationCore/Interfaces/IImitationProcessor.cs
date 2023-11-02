using SystemModeling.Lab2.Routing;

namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IImitationProcessor<TEvent>
{
    (Guid ThreadId, Task Task) GetProcessingTask(
        RoutingContext<TEvent> routingContext,
        CancellationToken ct);
}