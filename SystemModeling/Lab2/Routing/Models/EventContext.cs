namespace SystemModeling.Lab2.Routing.Models;

public class EventContext<TEvent>
{
    public required string EventId { get; init; }
    public required string NextProcessorName { get; set; }

    public required TEvent Event { get; set; }
}