namespace SystemModeling.Lab2.Options;

internal record EventProviderOptions
{
    public int EventsAmount { get; set; }
    public TimeSpan AddDelay { get; set; }
}