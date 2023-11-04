namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IObserver
{
    void Handle(IObservable observable);
}

internal interface IObserverTyped<in T>
{
    void Handle(T ctx);
}