using SystemModeling.Lab1.Analytics.Collectors;

namespace SystemModeling.Lab1.Analytics.Interfaces;

internal interface IChiCalculationStrategy
{
    double GetChi(SortedSet<FrequencyMapBucket> frequencyMap);
}

internal class NormalDistributionChiCalculationStrategy : IChiCalculationStrategy
{
    public double GetChi(SortedSet<FrequencyMapBucket> frequencyMap)
    {
        var theoreticalFrequency = (double)frequencyMap.Sum(x => x.ItemsAmount) / frequencyMap.Count;

        var chiSquare = 0d;
        foreach (var bucket in frequencyMap)
        {
            var realFrequency = bucket.ItemsAmount;
            chiSquare = Math.Pow(realFrequency - theoreticalFrequency, 2) / theoreticalFrequency;
        }

        return chiSquare;
    }
}
