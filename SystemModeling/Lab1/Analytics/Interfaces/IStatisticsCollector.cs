namespace SystemModeling.Lab1.Analytics.Interfaces;

internal interface IStatisticsCollector
{
    Task<object?> GetMetricAsync(AnalyticsContext ctx);
}