namespace SystemModeling.Lab2.Fluent.Interfaces;

internal interface IProcessorNodeBuilder
{
    IProcessorNodeBuilder AddTransition(string nextProcessor, double chance);
}