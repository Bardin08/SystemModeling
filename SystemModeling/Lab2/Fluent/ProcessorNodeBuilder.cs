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
            Transitions = new PriorityQueue<TransitionNode, int?>()
        };
        return this;
    }

    /// <inheritdoc />
    public IProcessorNodeBuilder SetMaxProcessorQueueLength(int maxLength = -1)
    {
        _processorNode.MaxQueueLength = maxLength;
        return this;
    }

    public IProcessorNodeBuilder AddTransition(string nextProcessor, double? chance = null, int? priority = null)
    {
        var transition = new TransitionNode()
        {
            Name = $"{_processorNode.Name}-->{nextProcessor}",
            ProcessorName = nextProcessor,
            TransitionChance = chance,
            Priority = priority
        };

        transition.Validate();

        _processorNode.Transitions.Enqueue(transition, priority);
        return this;
    }

    public ProcessorNode BuildNode()
    {
        return _processorNode;
    }
}