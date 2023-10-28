namespace SystemModeling.Lab2.Routing.Policies;

internal interface IRoutingPolicy
{
    Task RouteAsync(object? parameters, CancellationToken ct);
}