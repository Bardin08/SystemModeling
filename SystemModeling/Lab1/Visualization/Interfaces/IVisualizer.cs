namespace SystemModeling.Lab1.Visualization.Interfaces;

internal interface IVisualizer
{
    Task VisualizeAsync<TInput>(TInput input);
}