namespace SystemModeling.Lab1.Analytics.Interfaces;

internal interface IAnalyzingStep
{
    IAnalyzingStep? SetNext(IAnalyzingStep? next);
    Task<AnalyticsContext> AnalyzeAsync(AnalyticsContext context);
}