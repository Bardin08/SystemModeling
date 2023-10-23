using SystemModeling.Lab1.Analytics.Collectors.Options;
using SystemModeling.Lab1.Analytics.Interfaces;

namespace SystemModeling.Lab1.Analytics.Collectors;

internal class ChiSquareCollector : IStatisticsCollector
{
    private readonly DataFilteringOptions _dataFilteringOptions;

    public ChiSquareCollector(DataFilteringOptions dataFilteringOptions)
    {
        _dataFilteringOptions = dataFilteringOptions;
    }

    public Task<object?> GetMetricAsync(AnalyticsContext context)
    {
        if (context.FrequencyMap is null)
        {
            return Task.FromResult<object?>(null);
        }

        var filteredFrequencyMap = new SortedSet<FrequencyMapBucket>(
            Comparer<FrequencyMapBucket>.Default);
        foreach (var bucket in context.FrequencyMap
                     .Where(bucket => bucket.ItemsAmount >= _dataFilteringOptions.Threshold))
        {
            filteredFrequencyMap.Add(bucket);
        }

        var chiSquare = new NormalDistributionChiCalculationStrategy()
            .GetChi(filteredFrequencyMap);

        return Task.FromResult<object?>(chiSquare);
    }
}