namespace SystemModeling.Lab1.Analytics.Interfaces;

internal interface IAnalyticsProcessor
{
    Task<StatisticsDto?> AnalyzeAsync(double[] data);
}