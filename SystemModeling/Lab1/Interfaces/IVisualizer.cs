namespace SystemModeling.Lab1.Interfaces;

internal interface IVisualizer
{
    ValueTask VisualizeAsync(IEnumerable<double> data);
}