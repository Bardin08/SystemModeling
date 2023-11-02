using SystemModeling.Lab2.ImitationCore.Interfaces;

namespace SystemModeling.Lab2.ImitationCore.Observers;

internal class ProcessorEventConsumptionObserver : IObserver
{
    public void Handle(IObservable observable)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine("Event consumed!");
        Console.ResetColor();
    }
}