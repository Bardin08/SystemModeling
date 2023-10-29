namespace SystemModeling.Lab2.Models;

internal record ImitationContext
{
    public List<string> PassedHandlers { get; set; } = new();
}