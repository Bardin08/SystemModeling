namespace SystemModeling.Lab2.Models;

internal class ImitationStep
{
    public required string RouteId { get; init; }
    public string StepId { get; init; } = Guid.NewGuid().ToString()[..6];
}