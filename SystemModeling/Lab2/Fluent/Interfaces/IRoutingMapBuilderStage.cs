namespace SystemModeling.Lab2.Fluent.Interfaces;

internal interface IRoutingMapBuilderStage
{
    SimulationProcessorBuilder AndRoutingMap(Action<IRoutingMapBuilder> builder);
}