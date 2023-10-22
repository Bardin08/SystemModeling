namespace SystemModeling.Lab1.Visualization.Options;

internal record ConsoleVisualizationOptions : VisualizationOptionsBase
{
    public int Buckets { get; init; } = 20;
    public double MinValue { get; init; }
    public double MaxValue { get; init; } = 5;
    public int MaxCharsPerLine { get; init; } = 20;
    public VisualizationMode Mode { get; init; }
}