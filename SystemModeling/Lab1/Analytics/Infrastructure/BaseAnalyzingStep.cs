using SystemModeling.Lab1.Analytics.Interfaces;

namespace SystemModeling.Lab1.Analytics.Infrastructure;

internal abstract class BaseAnalyzingStep : IAnalyzingStep
{
    private IAnalyzingStep? _next;

    public IAnalyzingStep? SetNext(IAnalyzingStep? next)
    {
        _next = next;
        return next;
    }

    public virtual async Task<AnalyticsContext> AnalyzeAsync(AnalyticsContext context)
    {
        if (_next != null)
        {
            return await _next.AnalyzeAsync(context);
        }

        return context;
    }
}