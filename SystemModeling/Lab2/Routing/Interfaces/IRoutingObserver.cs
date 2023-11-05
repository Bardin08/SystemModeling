using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.ImitationCore.Observers;

namespace SystemModeling.Lab2.Routing.Interfaces;

internal interface IRoutingObserver<TEvent> : IObserverTyped<RoutingResult<TEvent>>
{
    ProcessorRoutingDescriptor AddAndGetProcessorStats(string processorName);
}