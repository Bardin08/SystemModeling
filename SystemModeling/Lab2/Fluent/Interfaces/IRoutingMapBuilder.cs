using SystemModeling.Lab2.Options;

namespace SystemModeling.Lab2.Fluent.Interfaces;

internal interface IRoutingMapBuilder
{
    IRoutingMapBuilder AddProcessor(
        string processorName, Action<IProcessorNodeBuilder> factory);

    void UseSingleConsumer(Action<ImitationProcessorOptions> factory);
    void UseMultipleConsumers(Action<MultiConsumersImitationProcessorOptions> factory);
}