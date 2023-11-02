using SystemModeling.Lab2.ImitationCore.Interfaces;

namespace SystemModeling.Lab2.ImitationCore.Observers;

internal class EventProcessorStateObserver : IObserver
{
    public void Handle(IObservable observable)
    {
        var processor = observable as IProcessor;
        ArgumentNullException.ThrowIfNull(processor);

        var sb = new StringBuilder();
        sb.Append($"ThreadId: {processor.ProcessorId}. Queue Size: {processor.QueueSize}");

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(sb.ToString());
        Console.ResetColor();
    }
}