using SystemModeling.Lab1.Analytics;
using SystemModeling.Lab1.Analytics.Interfaces;
using SystemModeling.Lab1.Generators.Interfaces;

namespace SystemModeling.Lab1.Fluent;

internal class Processor
{
    private readonly IGenerator _generator;
    private readonly IAnalyticsProcessor _analyticsProcessor;

    private double[]? _data;
    
    public Processor(
        IGenerator generator,
        IAnalyticsProcessor analyticsProcessor)
    {
        _generator = generator;
        _analyticsProcessor = analyticsProcessor;
    }

    public async Task Generate()
    {
        _data = await _generator.Generate();
    }

    public async Task<StatisticsDto?> Analyze()
    {
        ArgumentNullException.ThrowIfNull(_data);
        return await _analyticsProcessor.AnalyzeAsync(_data);
    }
}