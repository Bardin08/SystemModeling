using SystemModeling.Lab1.Analytics.Collectors.Options;
using SystemModeling.Lab1.Generators.Options;

namespace SystemModeling.Lab1.Analytics.Collectors.Helpers;

internal class NormalDistributionChiCalculationStrategy : ChiCalculationBaseStrategy
{
    public NormalDistributionChiCalculationStrategy(
        DataFilteringOptions dataFilteringOptions) : base(dataFilteringOptions)
    {
    }

    public override ChiSquareDto GetChi(AnalyticsContext context)
    {
        var requiredData = GetFrequencyMap(context.Data);

        var options = (context.GeneratorSettings as NormalDistributionOptions)!;
        var chiSquare = 0d;
        foreach (var (midPoint, observedFrequency) in requiredData.filteredFrequencyMap)
        {
            var firstPart = 1 / (Math.Sqrt(2 * Math.PI * Math.Pow(options.Sigma, 2)));
            var expPart = Math.Exp(-Math.Pow(midPoint - options.A, 2) / (2 * Math.Pow(options.Sigma, 2)));
            var theoreticalFrequency = requiredData.totalElements * firstPart * expPart * requiredData.intervalWidth;
            chiSquare += Math.Pow(observedFrequency - theoreticalFrequency, 2) / theoreticalFrequency;
        }   

        return new ChiSquareDto
        {
            ChiSquare = chiSquare,
            FreedomDegree = requiredData.freedomDegree
        };
    }

    public ChiSquareDto GetChi(Dictionary<double, int> frequencyMap)
    {
        throw new NotImplementedException();
    }
}