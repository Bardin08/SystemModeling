namespace SystemModeling.Lab1.Analytics;

internal interface IAnalyticsProcessor
{
    Task<StatisticsDto?> AnalyzeAsync(double[] data);
}