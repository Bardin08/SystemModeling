using SystemModeling.Lab2.ImitationCore.Backoffs;
using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Options.Backoffs;

namespace SystemModeling.Trumpee.Configuration;

public class TrumpeeSimulationOptions
{
    private static IBackoffStrategy DefaultBackoffStrategy
        => new NormalBackoff(
            new NormalBackoffOptions(
                TimeSpan.FromMilliseconds(20),
                TimeSpan.FromMilliseconds(70)));

    public static TrumpeeSimulationOptions Default => new()
    {
        DurationSeconds = 5,
        EventsGenerator = new EventsGeneratorOptions
        {
            Delay = TimeSpan.FromMilliseconds(1),
            TotalEventsAmount = 10,
            InitialProcessorName = "template-filling"
        },
        TemplateFilling = new ProcessorNodeOptions
        {
            MaxQueue = int.MaxValue,
            ProcessingTimeProvider = DefaultBackoffStrategy,
            RoutingFailureChance = Math.Pow(10, -5),
            ValidationFailureChance = 0.01
        },
        Validation = new ProcessorNodeOptions
        {
            MaxQueue = int.MaxValue,
            ProcessingTimeProvider = DefaultBackoffStrategy,
            RoutingFailureChance = Math.Pow(10, -5),
            ValidationFailureChance = 0.01
        },
        Prioritization = new ProcessorNodeOptions
        {
            MaxQueue = int.MaxValue,
            ProcessingTimeProvider = DefaultBackoffStrategy,
            RoutingFailureChance = Math.Pow(10, -5),
            ValidationFailureChance = 0.01
        },
        TransportHub = new ProcessorNodeOptions
        {
            MaxQueue = int.MaxValue,
            ProcessingTimeProvider = DefaultBackoffStrategy,
            RoutingFailureChance = Math.Pow(10, -5),
            ValidationFailureChance = 0.01
        }
    };

    public int DurationSeconds { get; init; }
    public EventsGeneratorOptions EventsGenerator { get; init; } = null!;
    public ProcessorNodeOptions Validation { get; init; } = null!;
    public ProcessorNodeOptions TemplateFilling { get; init; } = null!;
    public ProcessorNodeOptions Prioritization { get; init; } = null!;
    public ProcessorNodeOptions TransportHub { get; init; } = null!;
}

public class EventsGeneratorOptions
{
    /// <summary>
    /// Represents a delay between events or a seed value for events generator dispersion
    /// </summary>
    public TimeSpan Delay { get; init; }

    /// <summary>
    /// Represents total amount of events that will be generated
    /// </summary>
    public int TotalEventsAmount { get; init; }

    /// <summary>
    /// Represents very first processor at the simulation model
    /// </summary>
    public string InitialProcessorName { get; init; } = null!;
}

public class ProcessorNodeOptions
{
    /// <summary>
    /// Represents max queue length for validation node
    /// </summary>
    public int MaxQueue { get; init; }

    [JsonIgnore] public IBackoffStrategy ProcessingTimeProvider { get; set; } = null!;

    public double ValidationFailureChance { get; set; }
    public double RoutingFailureChance { get; set; }
    public double SuccessChance => 1 - ValidationFailureChance - RoutingFailureChance;
}