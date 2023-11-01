namespace SystemModeling.Lab2.Options;

internal record ImitationProcessorOptions
{
    public Guid ThreadId { get; set; }
    public TimeSpan ProcessingTime { get; init; }
    public string? Alias { get; init; }
    public ConsoleColor Color { get; init; }
    public int MaxQueueSize { get; set; }
}