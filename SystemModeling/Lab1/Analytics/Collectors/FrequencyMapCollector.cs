using SystemModeling.Lab1.Analytics.Collectors.Options;
using SystemModeling.Lab1.Analytics.Interfaces;

namespace SystemModeling.Lab1.Analytics.Collectors;

internal class FrequencyMapCollector : IStatisticsCollector
{
    private readonly FrequencyMapOptions _frequencyMapOptions;

    public FrequencyMapCollector(FrequencyMapOptions frequencyMapOptions)
    {
        _frequencyMapOptions = frequencyMapOptions;
    }

    public Task<object?> GetMetricAsync(AnalyticsContext context)
    {
        var data = context.Data;
        var min = data.Min();
        var max = data.Max();

        var bucketRange = (max - min) / _frequencyMapOptions.Buckets;
        var buckets = new int[_frequencyMapOptions.Buckets];

        foreach (var value in data)
        {
            var bucketIndex = (int)((value - min) / bucketRange);

            if (bucketIndex >= 0 && bucketIndex < _frequencyMapOptions.Buckets)
            {
                buckets[bucketIndex]++;
            }
        }

        var map = new SortedSet<FrequencyMapBucket>(
            Comparer<FrequencyMapBucket>.Create((lhs, rhs) => lhs.CompareTo(rhs)));

        for (var i = 0; i < _frequencyMapOptions.Buckets; i++)
        {
            var bucketMin = min + i * bucketRange;
            var bucketMax = bucketMin + bucketRange;
            var bucket = new FrequencyMapBucket()
            {
                Min = bucketMin,
                Max = bucketMax,
                ItemsAmount = buckets[i]
            };
            map.Add(bucket);
        }

        return Task.FromResult<object?>(map);
    }
}