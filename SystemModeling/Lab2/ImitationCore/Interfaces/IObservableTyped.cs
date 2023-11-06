namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IObservableTyped<TReturnType>
{
    void RegisterObserver(IObserverTyped<TReturnType> observer);
    void RemoveObserver(IObserverTyped<TReturnType> observer);
    void Notify(TReturnType observable);
}