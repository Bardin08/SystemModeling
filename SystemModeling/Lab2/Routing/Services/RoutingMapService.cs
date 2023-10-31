using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Routing.Services;

internal class RoutingMapService : IRoutingMapService
{
    private readonly List<ProcessorNode> _processors;

    public RoutingMapService(List<ProcessorNode> processors)
    {
        _processors = processors;
    }

    public ProcessorNode? GetProcessorNodeByName(string processorName)
    {
        var processorNode = _processors
            .FirstOrDefault(p => p.Name!.Equals(processorName, StringComparison.OrdinalIgnoreCase));

        return processorNode;
    }
}