namespace SystemModeling.Lab2.Routing.Models;

public class EventContext<TEvent>
{
    public string? EventId { get; set; }
    public string? NextProcessorName { get; set; }

    public TEvent? Event { get; set; }
}