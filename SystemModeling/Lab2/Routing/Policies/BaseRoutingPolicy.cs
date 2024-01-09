using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.ImitationCore.Observers;
using SystemModeling.Lab2.Routing.Interfaces;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Routing.Policies;

internal abstract class BaseRoutingPolicy<TEvent>(
    ChannelReader<EventContext<TEvent>> eventsStore,
    ConcurrentDictionary<string, Channel<EventContext<TEvent>>> handlers)
    : IRoutingPolicy<TEvent>
{
    private readonly List<IObserverTyped<RoutingResult<TEvent>>> _observers = new();

    protected readonly ChannelReader<EventContext<TEvent>> EventsStore = eventsStore;
    protected readonly ConcurrentDictionary<string, Channel<EventContext<TEvent>>> Handlers = handlers;

    public abstract Task RouteAsync(object? parameters, CancellationToken ct);

    public void RegisterObserver(IObserverTyped<RoutingResult<TEvent>> observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserverTyped<RoutingResult<TEvent>> observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(RoutingResult<TEvent> observable)
    {
        _observers.ForEach(o => o.Handle(observable));
    }
}