using System.Threading.Channels;
using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Routing;

public class RoutingContext<TEvent>
{
    public required ChannelReader<EventContext<TEvent>> ProcessorQueue { get; set; }
    public required ChannelWriter<EventContext<TEvent>> EventsSource { get; set; }
    public required object ProcessorOptions { get; set; }
}