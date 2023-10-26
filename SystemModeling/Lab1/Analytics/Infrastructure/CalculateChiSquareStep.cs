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

        if (chiSquare is ChiSquareDto chiSquareDto)
        {
            context.ChiSquare = chiSquareDto.ChiSquare;
            context.FreedomDegree = chiSquareDto.FreedomDegree;
        }

        return await base.AnalyzeAsync(context);
    }
}