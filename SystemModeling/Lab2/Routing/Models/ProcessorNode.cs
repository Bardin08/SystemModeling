namespace SystemModeling.Lab2.Routing.Models;

public class ProcessorNode
{
    public string? Name { get; init; }
    public string? RouteId { get; set; }
    public int MaxQueueLength { get; set; } = -1;

    public PriorityQueue<TransitionNode, int?> Transitions { get; init; } = new();
}