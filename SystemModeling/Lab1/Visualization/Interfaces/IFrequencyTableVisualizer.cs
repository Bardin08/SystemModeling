using SystemModeling.Lab1.Visualization.Options;

namespace SystemModeling.Lab1.Visualization.Interfaces;

internal interface IFrequencyTableVisualizer
{
    ValueTask VisualizeFrequencyTableAsync(double[] data, FrequencyTableVisualizerOptions options);
}