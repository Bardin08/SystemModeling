using SystemModeling.Lab1.Analytics.Collectors;
using SystemModeling.Lab1.Analytics.Interfaces;

namespace SystemModeling.Lab1.Analytics.Infrastructure;

internal class CalculateChiSquareStep : BaseAnalyzingStep
{
    private readonly IStatisticsCollector _collector;

    public CalculateChiSquareStep(IStatisticsCollector collector)
    {
        _collector = collector;
    }

    public override async Task<AnalyticsContext> AnalyzeAsync(AnalyticsContext context)
    {
        var chiSquare = await _collector.GetMetricAsync(context);
        var chiSquareDto = chiSquare as ChiSquareDto;

        if (chiSquareDto is not null)
        {
            context.ChiSquare = chiSquareDto.ChiSquare;
            context.FreedomDegree = chiSquareDto.FreedomDegree;
        }

        
        return await base.AnalyzeAsync(context);
    }
}