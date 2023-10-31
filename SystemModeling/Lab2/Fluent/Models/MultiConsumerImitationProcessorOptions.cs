using SystemModeling.Lab2.Options;

namespace SystemModeling.Lab2.Fluent.Models;

internal class MultiConsumerImitationProcessorOptions
{
    public List<ImitationProcessorOptions> ProcessorOptions { get; set; } = new();
    public int ConsumersAmount { get; set; }
}