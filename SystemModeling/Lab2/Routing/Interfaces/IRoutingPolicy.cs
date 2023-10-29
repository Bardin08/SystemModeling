namespace SystemModeling.Lab2.Routing.Interfaces;

internal interface IRoutingPolicy
{
    Task RouteAsync(object? parameters, CancellationToken ct);
}