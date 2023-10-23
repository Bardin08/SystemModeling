using SystemModeling.Lab1.Analytics.Collectors;

namespace SystemModeling.Lab1.Analytics;

internal record StatisticsDto
{
    public double Mean { get; init; }
    public double Variance { get; init; }
    public double ChiSquare { get; set; }
    public SortedSet<FrequencyMapBucket>? FrequencyMap { get; set; }

    internal static StatisticsDto FromAnalyzingContext(AnalyticsContext context)
    {
        return new StatisticsDto
        {
            FrequencyMap = context.FrequencyMap,
            Mean = context.Mean.GetValueOrDefault(),
            Variance = context.Variance.GetValueOrDefault(),
            ChiSquare = context.ChiSquare.GetValueOrDefault()
        };
    }
}