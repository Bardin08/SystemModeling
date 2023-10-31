using SystemModeling.Lab2.Fluent.Interfaces;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Fluent;

internal interface IProcessorNodeBuilder
{
    IProcessorNodeBuilder AddTransition(string nextProcessor, double chance);
}

internal class ProcessorNodeBuilder : IProcessorNodeBuilder
{
    private ProcessorNode _processorNode = new();

    public IProcessorNodeBuilder AddProcessor(string processor)
    {
        _processorNode = new ProcessorNode
        {
            Name = processor,
            Transitions = new List<TransitionNode>()
        };
        return this;
    }

    public IProcessorNodeBuilder AddTransition(string nextProcessor, double chance)
    {
        _processorNode.Transitions
            .Add(new TransitionNode
            {
                Name = $"{_processorNode.Name}-->{nextProcessor}",
                ProcessorName = nextProcessor,
                TransitionChance = chance
            });
        return this;
    }

    public ProcessorNode BuildNode()
    {
        return _processorNode;
    }
}

internal class RoutingMapBuilder : IRoutingMapBuilder
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

    public IRoutingMapBuilder AddProcessor(string processorName, Action<IProcessorNodeBuilder> factory)
    {
        var processorNodeBuilder = new ProcessorNodeBuilder();
        processorNodeBuilder.AddProcessor(processorName);
        factory(processorNodeBuilder);

        _currentProcessorNode = processorNodeBuilder.BuildNode();
        _processorNodes.Add(_currentProcessorNode);
        return this;
    }

    public void UseSingleConsumer(Action<ImitationProcessorOptions> factory)
    {
        if (_currentProcessorNode is null)
        {
            throw new Exception("There is no configured processors");
        }

        var options = new ImitationProcessorOptions();
        factory(options);
        _processorOptions.Add(_currentProcessorNode.Name!, options);
    }

    public void UseMultipleConsumers(
        Action<MultiConsumersImitationProcessorOptions> factory)
    {
        if (_currentProcessorNode is null)
        {
            throw new Exception("There is no configured processors");
        }

        var options = new MultiConsumersImitationProcessorOptions();
        factory(options);
        _processorOptions.Add(_currentProcessorNode.Name!, options);
    }

    public List<ProcessorNode> Build()
    {
        return _processorNodes;
    }
}