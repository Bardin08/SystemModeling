namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IObserver
{
    void Handle(IObservable observable);
}

internal interface IObservable
{
    void RegisterHandler(IObserver observer);
    void RemoveHandler(IObserver observer);
    void Notify();
}