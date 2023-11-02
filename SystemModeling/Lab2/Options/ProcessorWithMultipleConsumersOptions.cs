namespace SystemModeling.Lab2.Options;

internal record ProcessorWithMultipleConsumersOptions
{
    public List<ImitationProcessorOptions> ProcessorOptions { get; set; } = new();
    public int ConsumersAmount { get; set; }
}