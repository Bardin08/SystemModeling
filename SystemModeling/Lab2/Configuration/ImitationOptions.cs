namespace SystemModeling.Lab2.Configuration;

internal record ImitationOptions
{
    public TimeSpan ImitationTime { get; set; }
}

internal record ImitationThreadOptions
{
    public Guid ThreadId { get; init; }
    public TimeSpan SleepTime { get; init; }
}