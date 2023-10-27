namespace SystemModeling.Lab2.Configuration;

internal record ImitationThreadOptions
{
    public Guid ThreadId { get; init; }
    public TimeSpan ProcessingTime { get; init; }
}