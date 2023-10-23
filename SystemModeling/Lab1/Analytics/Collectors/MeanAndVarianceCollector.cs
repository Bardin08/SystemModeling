using SystemModeling.Lab1.Analytics.Interfaces;

namespace SystemModeling.Lab1.Analytics.Collectors;

internal class MeanAndVarianceCollector : IStatisticsCollector
{
    public Task<object?> GetMetricAsync(AnalyticsContext ctx)
    {
        var data = ctx.Data;

        var mean = data.Average();
        var variance = data.Aggregate(0d, (seed, item)
            => seed + (item - mean) * (item - mean)) / (data.Length - 1);

        return Task.FromResult<object?>((mean, variance));
    }
}