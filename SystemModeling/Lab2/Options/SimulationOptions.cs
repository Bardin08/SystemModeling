using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Options;

internal record SimulationOptions
{
    public TimeSpan SimulationTimeSeconds { get; set; }
    public Dictionary<string, object>? ProcessorDescriptors { get; set; } = new();
    public List<ProcessorNode>? RoutingMap { get; set; }
}