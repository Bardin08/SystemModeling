using SystemModeling.Lab1.Analytics.Collectors;

namespace SystemModeling.Lab1.Analytics;

internal record AnalyticsContext
{
    public double[] Data { get; init; } = Array.Empty<double>();
    public SortedSet<FrequencyMapBucket>? FrequencyMap { get; set; }
    public double? ChiSquare { get; set; }
    public int? FreedomDegree { get; set; }
    public double? Mean { get; set; }
    public double? Variance { get; set; }

    public object? GeneratorSettings { get; set; }
}