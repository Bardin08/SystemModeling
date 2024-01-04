namespace SystemModeling.Lab2.Routing.Models;

public class TransitionNode
{
    public required string Name { get; init; }
    public required string ProcessorName { get; init; }
    public double? TransitionChance { get; init; }
    public int? Priority { get; init; }

    public void Validate()
    {
        if (TransitionChance is null && Priority is null)
        {
            throw new ArgumentException(
                "You must setup priority or transition chance");
        }

        if (TransitionChance is not null && Priority is not null)
        {
            throw new ArgumentException(
                "You can't setup priority and transition chance at the same time");
        }
    }
}