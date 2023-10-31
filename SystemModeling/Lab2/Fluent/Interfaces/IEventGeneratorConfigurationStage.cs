using SystemModeling.Lab2.Options;

namespace SystemModeling.Lab2.Fluent.Interfaces;

internal interface IEventGeneratorConfigurationStage
{
    IRoutingMapBuilderStage WithEventGenerator(Action<EventProviderOptions> builder);
}