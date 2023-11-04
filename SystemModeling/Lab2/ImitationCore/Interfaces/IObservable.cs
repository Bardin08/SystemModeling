namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IObservable
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void Notify();
}

internal interface IObservableTyped<TReturnType>
{
    void RegisterObserver(IObserverTyped<TReturnType> observer);
    void RemoveObserver(IObserverTyped<TReturnType> observer);
    void Notify(TReturnType observable);
}