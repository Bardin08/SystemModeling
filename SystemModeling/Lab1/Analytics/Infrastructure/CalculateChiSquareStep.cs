using SystemModeling.Lab1.Analytics.Collectors;

namespace SystemModeling.Lab1.Analytics.Infrastructure;

internal class CalculateChiSquareStep : BaseAnalyzingStep
{
    private readonly ChiSquareCollector _collector;

    public CalculateChiSquareStep(ChiSquareCollector collector)
    {
        _collector = collector;
    }

    public override async Task<AnalyticsContext> AnalyzeAsync(AnalyticsContext context)
    {
        var chiSquare = await _collector.GetMetricAsync(context);
        context.ChiSquare = chiSquare as double?;

        return await base.AnalyzeAsync(context);
    }
}