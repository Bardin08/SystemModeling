using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.ImitationCore.Observers;

namespace SystemModeling.Lab2.Routing.Interfaces;

internal interface IRoutingPolicy<TEvent> : IObservableTyped<RoutingResult<TEvent>>
{
    Task RouteAsync(object? parameters, CancellationToken ct);
}