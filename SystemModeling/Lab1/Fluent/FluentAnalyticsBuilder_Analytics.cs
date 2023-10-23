using SystemModeling.Lab1.Analytics;
using SystemModeling.Lab1.Analytics.Interfaces;
using SystemModeling.Lab1.Fluent.Interfaces;

namespace SystemModeling.Lab1.Fluent;

internal partial class FluentProcessorBuilder : IAnalyticsStepsSelectionStage
{
    private IAnalyticsProcessor? _analyticsProcessor;
    
    public IProcessorBuilder AddCollectors(Action<AnalyticsProcessorBuilder> action)
    {
        var analyticsBuilder = new AnalyticsProcessorBuilder();

        action.Invoke(analyticsBuilder);
        _analyticsProcessor = analyticsBuilder.BuildProcessor();

        return this;
    }
}