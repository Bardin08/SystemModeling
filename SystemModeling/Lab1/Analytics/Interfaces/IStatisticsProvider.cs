namespace SystemModeling.Lab1.Analytics.Interfaces;

internal interface IStatisticsProvider
{
    Task<StatisticsDto> GetDatasetStatisticsAsync(double[] data);
}