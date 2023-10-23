using SystemModeling.Lab1.Analytics.Collectors;

namespace SystemModeling.Lab1.Analytics.Infrastructure;

internal class GetMeanAndVarianceStep : BaseAnalyzingStep
{
    private readonly MeanAndVarianceCollector _collector;

    public GetMeanAndVarianceStep(MeanAndVarianceCollector collector)
    {
        _collector = collector;
    }

    public override async Task<AnalyticsContext> AnalyzeAsync(AnalyticsContext context)
    {
        var meanAndVariance = await _collector.GetMetricAsync(context);
        var tuple = meanAndVariance as (double, double)? ?? (0, 0);

        context.Mean = tuple.Item1;
        context.Variance = tuple.Item2;

        return await base.AnalyzeAsync(context);
    }
}