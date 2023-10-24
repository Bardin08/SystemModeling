using SystemModeling.Lab1.Analytics;

namespace SystemModeling.Lab1.Fluent.Interfaces;

internal partial interface IAnalyticsStepsSelectionStage
{
    IProcessorBuilder AddCollectors(Action<AnalyticsProcessorBuilder> action);
}