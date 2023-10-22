using SystemModeling.Lab1.Analytics.Interfaces;

namespace SystemModeling.Lab1.Analytics;

internal class StatisticsProvider : IStatisticsProvider
{
    public Task<StatisticsDto> GetDatasetStatisticsAsync(double[] data)
    {
        var mean = data.Average();
        var variance = data.Aggregate(0d, (seed, item) => seed + (item - mean) * (item - mean)) / (data.Length - 1);

        return Task.FromResult(new StatisticsDto
        {
            Mean = mean,
            Variance = variance
        });
    }
}