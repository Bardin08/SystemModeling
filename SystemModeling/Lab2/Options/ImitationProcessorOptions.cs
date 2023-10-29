namespace SystemModeling.Lab2.Options;

internal record ImitationProcessorOptions
{
    public Guid ThreadId { get; set; }
    public TimeSpan ProcessingTime { get; init; }
}