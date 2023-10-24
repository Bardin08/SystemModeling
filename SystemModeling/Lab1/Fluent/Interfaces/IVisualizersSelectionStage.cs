using SystemModeling.Lab1.Visualization;

namespace SystemModeling.Lab1.Fluent.Interfaces;

internal interface IVisualizersSelectionStage
{
    IProcessorBuilder AddVisualizers(Action<VisualizationProcessorBuilder> action);
}