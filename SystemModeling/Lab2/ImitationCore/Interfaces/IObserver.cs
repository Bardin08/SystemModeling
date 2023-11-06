namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IObserver
{
    void Handle(IObservable observable);
}