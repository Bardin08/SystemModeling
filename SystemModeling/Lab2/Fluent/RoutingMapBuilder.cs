using SystemModeling.Lab2.Fluent.Interfaces;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Fluent;

internal class RoutingMapBuilder : IRoutingMapBuilder, IConsumersBuilderStage
{
    private readonly List<ProcessorNode> _processorNodes;
    private readonly Dictionary<string, object> _processorOptions;
    private ProcessorNode? _currentProcessorNode;

    private RoutingMapBuilder(Dictionary<string, object> processorOptions)
    {
        _processorOptions = processorOptions;
        _processorNodes = new List<ProcessorNode>();
    }

    public static RoutingMapBuilder CreateBuilder(Dictionary<string, object> processorOptions)
    {
        return new RoutingMapBuilder(processorOptions);
    }

    public IConsumersBuilderStage AddProcessor(string processorName, Action<IProcessorNodeBuilder> factory)
    {
        var processorNodeBuilder = new ProcessorNodeBuilder();
        processorNodeBuilder.AddProcessor(processorName);
        factory(processorNodeBuilder);

        _currentProcessorNode = processorNodeBuilder.BuildNode();
        _processorNodes.Add(_currentProcessorNode);
        return this;
    }

    public void UseConsumers(
        Action<ProcessorWithMultipleConsumersOptions> factory)
    {
        if (_currentProcessorNode is null)
        {
            throw new Exception("There is no configured processors");
        }

        var options = new ProcessorWithMultipleConsumersOptions();
        factory(options);
        _processorOptions.Add(_currentProcessorNode.Name!, options);
    }

    public List<ProcessorNode> Build()
    {
        return _processorNodes;
    }
}