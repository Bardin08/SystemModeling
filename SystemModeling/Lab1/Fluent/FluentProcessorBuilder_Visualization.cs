using SystemModeling.Lab1.Fluent.Interfaces;
using SystemModeling.Lab1.Visualization;

namespace SystemModeling.Lab1.Fluent;

internal partial class FluentProcessorBuilder : IVisualizersSelectionStage
{
    private VisualizationProcessor? _visualizationProcessor;

    public IProcessorBuilder AddVisualizers(Action<VisualizationProcessorBuilder> action)
    {
        var analyticsBuilder = VisualizationProcessorBuilder.CreateBuilder();

        action.Invoke(analyticsBuilder);
        _visualizationProcessor = analyticsBuilder.Build();

        return this;
    }
}