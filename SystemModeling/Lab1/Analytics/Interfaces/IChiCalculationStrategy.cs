using SystemModeling.Lab1.Analytics.Collectors.Options;
using SystemModeling.Lab1.Generators.Options;

namespace SystemModeling.Lab1.Analytics.Interfaces;

internal interface IChiCalculationStrategy
{
    ChiSquareDto GetChi(AnalyticsContext context);
}

internal class ChiSquareDto
{
    public double ChiSquare { get; init; }
    public int FreedomDegree { get; init; }
}

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