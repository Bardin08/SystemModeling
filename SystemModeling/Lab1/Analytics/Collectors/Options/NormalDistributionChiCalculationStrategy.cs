using SystemModeling.Lab1.Analytics.Interfaces;
using SystemModeling.Lab1.Generators.Options;

namespace SystemModeling.Lab1.Analytics.Collectors.Options;

internal class NormalDistributionChiCalculationStrategy : IChiCalculationStrategy
{
    private readonly DataFilteringOptions _dataFilteringOptions;

    public NormalDistributionChiCalculationStrategy(DataFilteringOptions dataFilteringOptions)
    {
        _dataFilteringOptions = dataFilteringOptions;
    }

    public ChiSquareDto GetChi(AnalyticsContext context)
    {
        var detailedFrequencyMap = context.Data
            .ToLookup(x => x, x => x + 1)
            .ToDictionary(x => x.Key, x => x.Count());

        var filteredFrequencyMap = new Dictionary<double, int>();
        foreach (var bucket in detailedFrequencyMap
                     .Where(bucket => bucket.Value >= _dataFilteringOptions.Threshold))
        {
            filteredFrequencyMap.TryAdd(bucket.Key, bucket.Value);
        }

        var options = (context.GeneratorSettings as NormalDistributionOptions)!;
        var totalAfterFilter = detailedFrequencyMap.Sum(x => x.Value);
        var width = detailedFrequencyMap.Keys.Max() -
                    detailedFrequencyMap.Keys.Min() / detailedFrequencyMap.Count;
        var freedomDegree = filteredFrequencyMap.Count - 1;
        var chiSquare = 0d;
        foreach (var (pivot, realFrequency) in detailedFrequencyMap)
        {
            var pt1 = 1 / Math.Sqrt(2 * Math.PI * Math.Pow(options.Sigma, 2));
            var pt2 = Math.Exp(-Math.Pow(pivot - options.A, 2) / 2 * Math.Pow(options.Sigma, 2));

            var theoreticalFrequency = totalAfterFilter * pt1 * pt2 * width;
            chiSquare += Math.Pow(realFrequency - theoreticalFrequency, 2) / theoreticalFrequency;
        }

        return new ChiSquareDto
        {
            ChiSquare = chiSquare,
            FreedomDegree = freedomDegree
        };
    }
}