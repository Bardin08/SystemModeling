namespace SystemModeling.Lab1.Visualization.Options;

internal record HistogramVisualizationOptions
{
    public int Buckets { get; set; } = 20;
    public double MinValue { get; set; }
    public double MaxValue { get; set; } = 5;
    public int MaxCharsPerLine { get; set; } = 20;
    public VisualizationMode Mode { get; set; }
}