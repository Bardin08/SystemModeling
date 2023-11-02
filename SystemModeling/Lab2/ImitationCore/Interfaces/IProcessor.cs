namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IProcessor
{
    public Guid ProcessorId { get; }
    public int QueueSize { get; }

    Task ProcessAsync(CancellationToken cancellationToken);
}