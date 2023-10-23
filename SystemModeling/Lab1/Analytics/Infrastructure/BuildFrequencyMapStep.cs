using SystemModeling.Lab1.Analytics.Collectors;

namespace SystemModeling.Lab1.Analytics.Infrastructure;

internal class BuildFrequencyMapStep : BaseAnalyzingStep
{
    private readonly FrequencyMapCollector _collector;

    public BuildFrequencyMapStep(FrequencyMapCollector collector)
    {
        _collector = collector;
    }

    public override async Task<AnalyticsContext> AnalyzeAsync(AnalyticsContext context)
    {
        var frequencyMap = await _collector.GetMetricAsync(context);
        context.FrequencyMap = frequencyMap as SortedSet<FrequencyMapBucket>;

        return await base.AnalyzeAsync(context);
    }
}