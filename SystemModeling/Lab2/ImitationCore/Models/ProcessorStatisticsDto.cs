namespace SystemModeling.Lab2.ImitationCore.Models;

internal record ProcessorStatisticsDto
{
    public required Guid ProcessorId { get; init; }
    public string? ProcessorName { get; set; }
    public int TotalQueueSize { get; init; }
    public int QueueSizeObservations { get; init; }
    public double TotalLoadTime { get; init; }
    public int LoadTimeObservations { get; init; }
    public double MeadQueueLength { get; init; }
    public double MeanLoadTime { get; init; }
}