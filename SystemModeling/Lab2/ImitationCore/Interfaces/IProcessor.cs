namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IProcessor
{
    Task ProcessAsync(CancellationToken cancellationToken);
}