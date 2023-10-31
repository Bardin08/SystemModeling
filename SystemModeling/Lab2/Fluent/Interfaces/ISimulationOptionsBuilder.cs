namespace SystemModeling.Lab2.Fluent.Interfaces;

internal interface ISimulationOptionsBuilder
{
    IEventGeneratorConfigurationStage ForSeconds(int seconds);
}