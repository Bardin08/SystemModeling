using System.Threading.Channels;

namespace SystemModeling.Lab2.Models;

internal record CreateImitationThreadResult<TEvent>
{
    public required Guid ThreadId { get; init; }
    public required ChannelWriter<TEvent> ChannelWriter { get; init; }
    public required Task ThreadExecutable { get; init; }
}