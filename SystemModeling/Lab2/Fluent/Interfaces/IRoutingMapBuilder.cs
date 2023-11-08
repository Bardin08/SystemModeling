using SystemModeling.Lab2.Options;

namespace SystemModeling.Lab2.Fluent.Interfaces;

internal interface IRoutingMapBuilder
{
    IConsumersBuilderStage AddProcessor(
        string processorName, Action<IProcessorNodeBuilder> factory);
}

internal interface IConsumersBuilderStage
{
    void UseSingleConsumer(Action<ImitationProcessorOptions> factory);
    void UseMultipleConsumers(Action<ProcessorWithMultipleConsumersOptions> factory);
}