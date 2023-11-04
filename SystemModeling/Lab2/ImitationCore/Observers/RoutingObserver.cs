using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.ImitationCore.Observers;

internal class RoutingObserver<TEvent> : IObserverTyped<RoutingResult<TEvent>>
{
    private int _totalRoutedEvents;
    private int _totalFailedEvents;

    public decimal MeanFailChance => (decimal)_totalRoutedEvents / _totalFailedEvents;

    public void Handle(RoutingResult<TEvent> ctx)
    {
        _totalRoutedEvents++;

        if (!ctx.IsSuccess)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Event wasn't passed to the {0}. {1}", ctx.TargetedProcessor, ctx.EventContext.Event);
            Console.ResetColor();

            _totalFailedEvents++;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Event was passed to the {0}. {1}", ctx.TargetedProcessor, ctx.EventContext.Event);
            Console.ResetColor();
        }
    }
}

internal record RoutingResult<TEvent>
{
    public bool IsSuccess { get; init; }
    public required string TargetedProcessor { get; init; }
    public required EventContext<TEvent> EventContext { get; init; }
}