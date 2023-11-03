﻿namespace SystemModeling.Lab2.ImitationCore.Models;

internal record ProcessorStatisticsDto
{
    public int TotalQueueSize { get; init; }
    public int QueueSizeObservations { get; init; }
    public double TotalLoadTime { get; init; }
    public int LoadTimeObservations { get; init; }
}