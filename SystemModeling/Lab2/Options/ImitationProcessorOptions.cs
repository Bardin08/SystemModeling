using SystemModeling.Lab2.ImitationCore.Interfaces;

namespace SystemModeling.Lab2.ImitationCore.Backoffs;

internal record ImitationProcessorOptions
{
    public Guid ThreadId { get; set; }
    public IBackoffStrategy? ProcessingTimeProvider { get; set; }
    public TimeSpan ProcessingTime { get; set; }
    public string? Alias { get; set; }
    public ConsoleColor Color { get; set; }
    public int MaxQueueSize { get; set; }
}