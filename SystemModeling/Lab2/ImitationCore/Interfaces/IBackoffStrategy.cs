namespace SystemModeling.Lab2.ImitationCore.Interfaces;

public interface IBackoffStrategy
{
    TimeSpan GetBackoff();
}