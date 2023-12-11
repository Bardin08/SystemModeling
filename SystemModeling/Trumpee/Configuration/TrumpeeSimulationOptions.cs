namespace SystemModeling.Trumpee.Configuration;

public class TrumpeeSimulationOptions
{
    public static TrumpeeSimulationOptions Default => new()
    {
        DurationSeconds = 300,
        EventsGenerator = new EventsGeneratorOptions
        {
            Delay = TimeSpan.FromSeconds(1),
            TotalEventsAmount = int.MaxValue
        },
        Validation = new ValidationNodeOptions
        {
            MaxQueue = int.MaxValue,
            AverageValidationTime = TimeSpan.FromSeconds(0.5),
            RoutingFailureChance = Math.Pow(10, -5),
            ValidationFailureChance = 0.01
        }
    };

    public int DurationSeconds { get; init; }
    public EventsGeneratorOptions EventsGenerator { get; init; } = null!;
    public ValidationNodeOptions Validation { get; init; } = null!;
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
}

public class ValidationNodeOptions
{
    /// <summary>
    /// Represents max queue length for validation node
    /// </summary>
    public int MaxQueue { get; init; }

    /// <summary>
    /// Represents an average time required to validate one event
    /// </summary>
    public TimeSpan AverageValidationTime { get; init; }

    public double ValidationFailureChance { get; set; }
    public double RoutingFailureChance { get; set; }
    public double SuccessChance => 1 - ValidationFailureChance - RoutingFailureChance;
}