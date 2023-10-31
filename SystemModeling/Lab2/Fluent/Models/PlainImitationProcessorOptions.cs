using SystemModeling.Lab2.Options;

namespace SystemModeling.Lab2.Fluent.Models;

internal class PlainImitationProcessorOptions
{
    public List<ImitationProcessorOptions> Options { get; set; } = new();
}