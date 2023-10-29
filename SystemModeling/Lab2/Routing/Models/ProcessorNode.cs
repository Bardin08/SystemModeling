namespace SystemModeling.Lab2.Routing.Models;

public class ProcessorNode
{
    public required string Name { get; init; }
    public required string RouteId { get; init; }

    public List<TransitionNode> Transitions { get; init; } = new();
}