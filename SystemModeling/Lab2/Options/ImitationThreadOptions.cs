namespace SystemModeling.Lab2.Options;

internal record ImitationThreadOptions
{
    public Guid ThreadId { get; init; }
    public TimeSpan ProcessingTime { get; init; }
}