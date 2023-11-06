namespace SystemModeling.Lab2.ImitationCore.Interfaces;

public interface IObserverTyped<in T>
{
    void Handle(T ctx);
}