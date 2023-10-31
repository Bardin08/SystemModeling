using SystemModeling.Lab1.Analytics.Interfaces;

namespace SystemModeling.Lab1.Analytics;

internal class AnalyticsProcessor : IAnalyticsProcessor
{
    private readonly IAnalyzingStep _analyzingSteps;

    public AnalyticsProcessor(IAnalyzingStep analyzingSteps)
    {
        _analyzingSteps = analyzingSteps;
    }

    public async Task<StatisticsDto?> AnalyzeAsync(double[]? data, object generatorSettings)
    {
        if (data is null || !data.Any())
        {
            return null;
        }

        var ctx = new AnalyticsContext()
        {
            Data = data,
            GeneratorSettings = generatorSettings
        };

        return StatisticsDto.FromAnalyzingContext(
            await AnalyzeInternalAsync(ctx));
    }

    private async Task<AnalyticsContext> AnalyzeInternalAsync(AnalyticsContext context)
    {
        return await _analyzingSteps.AnalyzeAsync(context);
    }
}