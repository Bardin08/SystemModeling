using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Observers;

internal record RoutingResult<TEvent>
{
    public bool IsSuccess { get; init; }
    public required string TargetedProcessor { get; init; }
    public required EventContext<TEvent> EventContext { get; init; }
}