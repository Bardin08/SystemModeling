using SystemModeling.Lab2.Fluent.Interfaces;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Fluent;

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

    /// <inheritdoc />
    public IProcessorNodeBuilder SetMaxLength(int maxLength = -1)
    {
        _processorNode.MaxQueueLength = maxLength;
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