using SystemModeling.Lab1.Analytics.Collectors;
using SystemModeling.Lab1.Analytics.Collectors.Options;
using SystemModeling.Lab1.Analytics.Infrastructure;
using SystemModeling.Lab1.Analytics.Interfaces;

namespace SystemModeling.Lab1.Analytics;

internal class AnalyticsProcessorBuilder
{
    private readonly HashSet<IAnalyzingStep> _steps = new();

    public AnalyticsProcessorBuilder BuildFrequencyMap(FrequencyMapOptions options)
    {
        _steps.Add(new BuildFrequencyMapStep(new FrequencyMapCollector(options)));
        return this;
    }

    public AnalyticsProcessorBuilder CalculateChiSquare(DataFilteringOptions options)
    {
        _steps.Add(new CalculateChiSquareStep(new ChiSquareCollector(options)));
        return this;
    }

    public AnalyticsProcessorBuilder CalculateMeanAndVariance()
    {
        _steps.Add(new GetMeanAndVarianceStep(new MeanAndVarianceCollector()));
        return this;
    }

    public AnalyticsProcessor BuildProcessor()
    {
        var first = _steps.First();
        _steps.Aggregate((a, b) => a.SetNext(b)!);
        return new AnalyticsProcessor(first);
    }
}