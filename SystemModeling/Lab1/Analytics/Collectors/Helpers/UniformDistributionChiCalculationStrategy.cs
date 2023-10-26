using SystemModeling.Lab1.Analytics.Collectors.Options;

namespace SystemModeling.Lab1.Analytics.Collectors.Helpers;

internal class UniformDistributionChiCalculationStrategy : ChiCalculationBaseStrategy
{
    public UniformDistributionChiCalculationStrategy(
        DataFilteringOptions dataFilteringOptions) : base(dataFilteringOptions)
    {
    }

    public override ChiSquareDto GetChi(AnalyticsContext context)
    {
        var requiredData = GetFrequencyMap(context.Data);
        var theoreticalFrequency = requiredData.totalElements / requiredData.filteredIntervals;

        var chiSquare = 0d;
        foreach (var item in requiredData.filteredFrequencyMap)
        {
            chiSquare = Math.Pow(item.Value - theoreticalFrequency, 2) / theoreticalFrequency;
        }

        return new ChiSquareDto
        {
            ChiSquare = chiSquare,
            FreedomDegree = requiredData.freedomDegree
        };
    }
}