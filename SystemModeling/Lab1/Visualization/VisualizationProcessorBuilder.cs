using SystemModeling.Lab1.Analytics;
using SystemModeling.Lab1.Analytics.Collectors;
using SystemModeling.Lab1.Visualization.Console;
using SystemModeling.Lab1.Visualization.Interfaces;
using SystemModeling.Lab1.Visualization.Options;

namespace SystemModeling.Lab1.Visualization;

internal sealed class VisualizationProcessorBuilder
{
    private readonly Dictionary<Type, HashSet<IVisualizer>> _visualizers;

    private VisualizationProcessorBuilder()
    {
        _visualizers = new Dictionary<Type, HashSet<IVisualizer>>();
    }

    public static VisualizationProcessorBuilder CreateBuilder()
    {
        return new VisualizationProcessorBuilder();
    }

    public VisualizationProcessorBuilder AddStatisticsVisualization()
    {
        if (_visualizers.TryGetValue(typeof(StatisticsDto), out var value))
        {
            value.Add(new StatisticsVisualizer());
        }
        else
        {
            _visualizers.TryAdd(typeof(StatisticsDto),
                new HashSet<IVisualizer> { new StatisticsVisualizer() });
        }

        return this;
    }

    public VisualizationProcessorBuilder AddHistogramVisualization(
        Action<HistogramVisualizationOptions> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        var options = new HistogramVisualizationOptions();
        factory(options);

        if (_visualizers.TryGetValue(typeof(SortedSet<FrequencyMapBucket>), out var value))
        {
            value.Add(new HistogramVisualizer(options));
        }
        else
        {
            _visualizers.TryAdd(typeof(SortedSet<FrequencyMapBucket>),
                new HashSet<IVisualizer> { new HistogramVisualizer(options) });
        }

        return this;
    }

    public VisualizationProcessorBuilder AddFrequencyMapVisualization()
    {
        if (_visualizers.TryGetValue(typeof(SortedSet<FrequencyMapBucket>), out var value))
        {
            value.Add(new FrequencyMapVisualizer());
        }
        else
        {
            _visualizers.TryAdd(typeof(SortedSet<FrequencyMapBucket>),
                new HashSet<IVisualizer> { new FrequencyMapVisualizer() });
        }

        return this;
    }

    public VisualizationProcessor Build()
    {
        return new VisualizationProcessor(_visualizers);
    }
}