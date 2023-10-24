namespace SystemModeling.Lab1.Analytics.Interfaces;

internal interface IChiCalculationStrategy
{
    ChiSquareDto GetChi(AnalyticsContext context);
}