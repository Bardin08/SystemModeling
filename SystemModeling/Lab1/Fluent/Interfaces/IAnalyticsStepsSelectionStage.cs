using SystemModeling.Lab1.Analytics;

namespace SystemModeling.Lab1.Fluent.Interfaces;

internal interface IAnalyticsStepsSelectionStage
{
    IProcessorBuilder AddCollectors(Action<AnalyticsProcessorBuilder> action);
}