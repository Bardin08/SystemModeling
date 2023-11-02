namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IObservable
{
    void RegisterHandler(IObserver observer);
    void RemoveHandler(IObserver observer);
    void Notify();
}