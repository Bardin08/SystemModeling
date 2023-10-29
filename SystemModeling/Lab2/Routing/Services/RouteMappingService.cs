using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Routing.Services;

internal class RouteMappingService
{
    private readonly List<ProcessorNode> _processors;

    public RouteMappingService(List<ProcessorNode> processors)
    {
        _processors = processors;
    }

    public ProcessorNode GetProcessorNodeByName(string processorName)
    {
        var processorNode = _processors
            .First(p => p.Name.Equals(processorName, StringComparison.OrdinalIgnoreCase));

        return processorNode;
    }
}