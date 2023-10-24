using SystemModeling.Lab1.Analytics.Collectors.Options;
using SystemModeling.Lab1.Analytics.Interfaces;
using SystemModeling.Lab1.Generators.Options;

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

        var chiSquare = context.GeneratorSettings switch
        {
            UniformDistributionOptions => new UniformDistributionChiCalculationStrategy(_dataFilteringOptions)
                .GetChi(context),
            NormalDistributionOptions => new NormalDistributionChiCalculationStrategy(_dataFilteringOptions)
                .GetChi(context),
            _ => new ExponentialDistributionChiCalculationStrategy(_dataFilteringOptions)
                .GetChi(context)
        };

        return Task.FromResult<object?>(chiSquare);
    }
}