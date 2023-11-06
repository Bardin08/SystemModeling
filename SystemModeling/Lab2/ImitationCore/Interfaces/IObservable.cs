namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IObservable
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void Notify();
}