using SystemModeling.Lab1.Analytics.Collectors.Options;
using SystemModeling.Lab1.Analytics.Interfaces;

namespace SystemModeling.Lab1.Analytics.Collectors.Helpers;

internal abstract class ChiCalculationBaseStrategy : IChiCalculationStrategy
{
    private readonly DataFilteringOptions _dataFilteringOptions;

    protected ChiCalculationBaseStrategy(DataFilteringOptions dataFilteringOptions)
    {
        _dataFilteringOptions = dataFilteringOptions;
    }

    public abstract ChiSquareDto GetChi(AnalyticsContext context);

    protected (Dictionary<double, int> filteredFrequencyMap, int freedomDegree, int totalElements,
        int filteredIntervals, double intervalWidth) GetFrequencyMap(double[] data)
    {
        const int numberOfIntervals = 100;

        var min = data.Min();
        var max = data.Max();

        var intervalWidth = (max - min) / numberOfIntervals;

        var frequencyMap = new Dictionary<double, int>();
        foreach (var num in data)
        {
            var midPoint = Math.Floor((num - min) / intervalWidth) * intervalWidth + intervalWidth / 2 + min;

            if (frequencyMap.ContainsKey(midPoint))
            {
                frequencyMap[midPoint] += 1;
            }
            else
            {
                frequencyMap[midPoint] = 1;
            }
        }

        var filteredIntervals = 0;
        var totalElements = 0;

        var filteredFrequencyMap = new Dictionary<double, int>();
        foreach (var entry in frequencyMap.Where(entry => entry.Value >= _dataFilteringOptions.Threshold))
        {
            filteredFrequencyMap.Add(entry.Key, entry.Value);
            totalElements += entry.Value;
            filteredIntervals++;
        }

        var degreesOfFreedom = filteredIntervals - 1;
        return (filteredFrequencyMap, degreesOfFreedom, totalElements, filteredIntervals, intervalWidth);
    }
}