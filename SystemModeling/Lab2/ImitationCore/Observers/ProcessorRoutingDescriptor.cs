﻿namespace SystemModeling.Lab2.ImitationCore.Observers;

internal class ProcessorRoutingDescriptor
{
    public required string ProcessorName { get; set; }
    public int TotalRoutedEvents { get; set; }
    public int FailedEvents { get; set; }

    public int SuccessEvents => TotalRoutedEvents - FailedEvents;
    public decimal MeanFailChance => (decimal)FailedEvents / SuccessEvents;
}