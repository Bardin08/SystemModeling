namespace SystemModeling.Lab2.Fluent.Interfaces;

internal interface ISimulationOptionsBuilder
{
    IRoutingMapBuilderStage ForSeconds(int seconds);
}