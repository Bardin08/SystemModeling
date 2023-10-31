using SystemModeling.Lab2.Options;

namespace SystemModeling.Lab2.Fluent.Interfaces;

internal interface IRoutingMapBuilderStage
{
    SimulationProcessorBuilder AndRoutingMap(Action<IRoutingMapBuilder> builder);
    SimulationProcessorBuilder WithEventGenerator(Action<EventProviderOptions> builder);
}