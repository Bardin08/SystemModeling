using SystemModeling.Lab1.Generators.Options;

namespace SystemModeling.Lab1.Analytics.Collectors.Options;

internal class ExponentialDistributionChiCalculationStrategy : ChiCalculationBaseStrategy
{
    public ExponentialDistributionChiCalculationStrategy(
        DataFilteringOptions dataFilteringOptions) : base(dataFilteringOptions)
    {
    }

    public override ChiSquareDto GetChi(AnalyticsContext context)
    {
        var requiredData = GetFrequencyMap(context.Data);

        var chiSquare = 0d;
        var options = (context.GeneratorSettings as ExponentialDistributionOptions)!;
        foreach (var (pivot, observedFrequency) in requiredData.filteredFrequencyMap)
        {
            var theoreticalFrequency = requiredData.totalElements * (Math.Exp(-options.Lambda * pivot) -
                                                                     Math.Exp(-options.Lambda *
                                                                              (pivot + requiredData.intervalWidth)));
            chiSquare += Math.Pow(observedFrequency - theoreticalFrequency, 2) / theoreticalFrequency;
        }

        return new ChiSquareDto
        {
            ChiSquare = chiSquare,
            FreedomDegree = requiredData.freedomDegree
        };
    }
}