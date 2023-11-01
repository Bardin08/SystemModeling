using SystemModeling.Lab2.Routing;

namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IImitationProcessorFactory<TEvent>
{
    (Guid ThreadId, Task Task) GetProcessingTask(
        RoutingContext<TEvent> routingContext,
        CancellationToken ct);
}