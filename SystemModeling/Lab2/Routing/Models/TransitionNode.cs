namespace SystemModeling.Lab2.Routing.Models;

public class TransitionNode
{
    public required string Name { get; init; }
    public required string ProcessorName { get; init; }
    public double TransitionChance { get; init; }
}