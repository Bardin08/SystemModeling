namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IObserver
{
    void Handle(IObservable observable);
}

public interface IObserverTyped<in T>
{
    void Handle(T ctx);
}