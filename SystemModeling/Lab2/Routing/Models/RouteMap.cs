namespace SystemModeling.Lab2.Routing.Models;

internal class RouteMap
{
    private readonly List<ProcessorNode> _processors;

    public RouteMap(List<ProcessorNode> processors)
    {
        _processors = processors;
    }

    public ProcessorNode GetProcessorNodeByName(string processorName)
    {
        var processorNode = _processors
            .First(p => p.Name.Equals(processorName, StringComparison.OrdinalIgnoreCase));

        return processorNode;
    }

    public List<TransitionNode> GetTransitionNodesByProcessorName(string processorName)
    {
        var processorNode = _processors
            .First(p => p.Name.Equals(processorName, StringComparison.OrdinalIgnoreCase));

        return processorNode.Transitions;
    }
}

public class ProcessorNode
{
    public required string Name { get; set; }
    public required string RouteId { get; set; }

    public List<TransitionNode> Transitions { get; set; } = new();
}

public class TransitionNode
{
    public required string Name { get; set; }
    public required string TargetProcessorRouteId { get; set; }
    public required string ProcessorName { get; set; }
    public double TransitionChance { get; init; }
}