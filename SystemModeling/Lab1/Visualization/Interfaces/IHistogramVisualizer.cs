using SystemModeling.Lab1.Visualization.Options;

namespace SystemModeling.Lab1.Visualization.Interfaces;

internal interface IHistogramVisualizer
{
    ValueTask VisualizeHistogramAsync(double[] data, HistogramVisualizationOptions options);
}