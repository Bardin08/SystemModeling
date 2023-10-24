using SystemModeling.Lab1.Analytics.Interfaces;
using SystemModeling.Lab1.Generators.Options;

namespace SystemModeling.Lab1.Analytics.Collectors.Options;

internal class ExponentialDistributionChiCalculationStrategy : IChiCalculationStrategy
{
    private readonly DataFilteringOptions _dataFilteringOptions;

    public ExponentialDistributionChiCalculationStrategy(DataFilteringOptions dataFilteringOptions)
    {
        _dataFilteringOptions = dataFilteringOptions;
    }

    public ChiSquareDto GetChi(AnalyticsContext context)
    {
        var filteredFrequencyMap = new Dictionary<double, int>();
        foreach (var bucket in context.FrequencyMap!
                     .Where(bucket => bucket.ItemsAmount >= _dataFilteringOptions.Threshold))
        {
            filteredFrequencyMap.TryAdd((bucket.Min + bucket.Max) / 2, bucket.ItemsAmount);
        }

        var options = (context.GeneratorSettings as ExponentialDistributionOptions)!;
        var totalAfterFilter = context.FrequencyMap!.Sum(x => x.ItemsAmount);
        var width = filteredFrequencyMap.Max(x => x.Key) -
                    filteredFrequencyMap.Min(x => x.Key) /context.FrequencyMap!.Count;
        var freedomDegree = filteredFrequencyMap.Count - 1;

        var chiSquare = 0d;
        foreach (var (pivot, realFrequency) in filteredFrequencyMap)
        {
            var theoreticalFrequency = totalAfterFilter *
                                       (Math.Exp(-options.Lambda * pivot) -
                                        Math.Exp(-options.Lambda * (pivot + width)));
            chiSquare += Math.Pow(realFrequency - theoreticalFrequency, 2) / theoreticalFrequency;
        }

        return new ChiSquareDto
        {
            ChiSquare = chiSquare,
            FreedomDegree = freedomDegree
        };
    }
}