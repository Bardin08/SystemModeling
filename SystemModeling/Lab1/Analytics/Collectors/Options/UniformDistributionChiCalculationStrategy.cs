using SystemModeling.Lab1.Analytics.Interfaces;

namespace SystemModeling.Lab1.Analytics.Collectors.Options;

internal class UniformDistributionChiCalculationStrategy : IChiCalculationStrategy
{
    private readonly DataFilteringOptions _dataFilteringOptions;

    public UniformDistributionChiCalculationStrategy(DataFilteringOptions dataFilteringOptions)
    {
        _dataFilteringOptions = dataFilteringOptions;
    }

    public ChiSquareDto GetChi(AnalyticsContext context)
    {
        var detailedFrequencyMap = context.Data
            .ToLookup(x => x, x => x + 1)
            .ToDictionary(x => x.Key, x => x.Count());

        var theoreticalFrequency = detailedFrequencyMap.Sum(x => x.Value) / detailedFrequencyMap.Count;

        var filteredFrequencyMap = new Dictionary<double, int>();
        foreach (var bucket in detailedFrequencyMap
                     .Where(bucket => bucket.Value >= _dataFilteringOptions.Threshold))
        {
            filteredFrequencyMap.TryAdd(bucket.Key, bucket.Value);
        }

        var freedomDegree = filteredFrequencyMap.Count - 1;
        var chiSquare = 0d;
        foreach (var realFrequency in filteredFrequencyMap.Select(bucket => bucket.Value))
        {
            chiSquare = Math.Pow(realFrequency - theoreticalFrequency, 2) / theoreticalFrequency;
        }

        return new ChiSquareDto
        {
            ChiSquare = chiSquare,
            FreedomDegree = freedomDegree
        };
    }
}