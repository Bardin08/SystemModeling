namespace SystemModeling.Lab1.Fluent.Interfaces;

internal interface IProcessorBuilder
{
    IDatasetGeneratorSelectionStage Generate();
    IAnalyticsStepsSelectionStage Analyze();
    Processor Build();
}