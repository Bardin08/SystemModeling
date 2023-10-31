using SystemModeling.Lab1.Analytics;
using SystemModeling.Lab1.Analytics.Interfaces;
using SystemModeling.Lab1.Generators.Interfaces;
using SystemModeling.Lab1.Visualization;

namespace SystemModeling.Lab1.Fluent;

internal class Processor
{
    private readonly IGenerator _generator;
    private readonly IAnalyticsProcessor _analyticsProcessor;
    private readonly VisualizationProcessor _visualizationProcessor;

    private double[]? _data;

    public Processor(IGenerator generator,
        IAnalyticsProcessor analyticsProcessor,
        VisualizationProcessor visualizationProcessor)
    {
        _generator = generator;
        _analyticsProcessor = analyticsProcessor;
        _visualizationProcessor = visualizationProcessor;
    }

    public async Task Generate()
    {
        _data = await _generator.Generate();
    }

    public async Task<StatisticsDto?> Analyze()
    {
        ArgumentNullException.ThrowIfNull(_data);
        return await _analyticsProcessor.AnalyzeAsync(_data, _generator.GetOptions());
    }

    public async Task Visualize<TData>(TData data)
    {
        ArgumentNullException.ThrowIfNull(data);
        await _visualizationProcessor.VisualizeAsync(data);
    }
}