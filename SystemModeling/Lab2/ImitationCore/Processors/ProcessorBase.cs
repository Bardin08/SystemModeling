using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Processors;

internal abstract class ProcessorBase<TEvent> :
    IProcessor, IObservable
{
    public Guid ProcessorId { get; }
    public int QueueSize => ProcessorQueue.Count;
    public TimeSpan ProcessingTime { get; set; }

    private readonly List<IObserver> _observers = new();

    protected readonly ChannelWriter<EventContext<TEvent>> RouterQueue;
    protected readonly ChannelReader<EventContext<TEvent>> ProcessorQueue;
    protected readonly CancellationTokenSource CancellationTokenSource;

    public abstract Task ProcessAsync(CancellationToken cancellationToken);

    protected ProcessorBase(
        ChannelWriter<EventContext<TEvent>> routerQueue,
        ChannelReader<EventContext<TEvent>> processorQueue,
        CancellationTokenSource cancellationTokenSource)
    {
        RouterQueue = routerQueue;
        ProcessorQueue = processorQueue;
        CancellationTokenSource = cancellationTokenSource;

        ProcessorId = Guid.NewGuid();
    }

    public void RegisterHandler(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveHandler(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        _observers.ForEach(o => o.Handle(this));
    }
}