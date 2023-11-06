using SystemModeling.Lab2.Routing.Interfaces;

namespace SystemModeling.Lab2.ImitationCore.Observers;

internal class RoutingObserver<TEvent> : IRoutingObserver<TEvent>
{
    private readonly ConcurrentDictionary<string, ProcessorRoutingDescriptor> _descriptors = new();

    public void Handle(RoutingResult<TEvent> ctx)
    {
        if (!_descriptors.TryGetValue(ctx.TargetedProcessor, out var descriptor))
        {
            descriptor = new ProcessorRoutingDescriptor
            {
                ProcessorName = ctx.TargetedProcessor,
                TotalRoutedEvents = 1
            };
            _descriptors.TryAdd(ctx.TargetedProcessor, descriptor);
        }

        descriptor.TotalRoutedEvents++;

        if (!ctx.IsSuccess)
        {
            descriptor.FailedEvents++;
        }
    }

    public ProcessorRoutingDescriptor AddAndGetProcessorStats(string processorName)
    {
        var descriptor = new ProcessorRoutingDescriptor
        {
            ProcessorName = processorName
        };

        _descriptors.TryAdd(processorName, descriptor);

        return _descriptors[processorName];
    }
}