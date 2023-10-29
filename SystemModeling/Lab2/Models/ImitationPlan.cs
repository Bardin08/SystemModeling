namespace SystemModeling.Lab2.Models;

internal record ImitationPlan
{
    public List<ImitationStep> Steps { get; init; } = new();
}