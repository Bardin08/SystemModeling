namespace SystemModeling.Lab2.Options;

internal record ProcessorWithMultipleConsumersOptions
{
    public List<ImitationProcessorOptions> ProcessorOptions { get; set; } = [];
    public int ConsumersAmount => ProcessorOptions.Count;
}