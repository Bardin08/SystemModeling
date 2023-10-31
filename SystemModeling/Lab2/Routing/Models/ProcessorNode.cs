namespace SystemModeling.Lab2.Routing.Models;

public class ProcessorNode
{
    public string? Name { get; init; }
    public string? RouteId { get; set; }

    public List<TransitionNode> Transitions { get; init; } = new();
}