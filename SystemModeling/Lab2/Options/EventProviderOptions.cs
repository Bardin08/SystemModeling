using SystemModeling.Lab2.ImitationCore.Interfaces;

namespace SystemModeling.Lab2.Options;

internal record EventProviderOptions
{
    public int EventsAmount { get; set; }
    public IBackoffStrategy BackoffProvider { get; set; } = null!;
    public string ProcessorName { get; set; } = null!;
}