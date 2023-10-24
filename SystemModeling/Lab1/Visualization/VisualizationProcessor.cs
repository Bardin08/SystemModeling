using SystemModeling.Lab1.Visualization.Interfaces;

namespace SystemModeling.Lab1.Visualization;

internal class VisualizationProcessor
{
    private Dictionary<Type, HashSet<IVisualizer>> _visualizers;

    public VisualizationProcessor(Dictionary<Type, HashSet<IVisualizer>> visualizers)
    {
        _visualizers = visualizers;
    }

    public async Task VisualizeAsync<TData>(TData data)
    {
        if (_visualizers.TryGetValue(data!.GetType(), out var visualizers))
        {
            foreach (var visualizer in visualizers)
            {
                await visualizer.VisualizeAsync(data);
                System.Console.WriteLine();                
            }

            return;
        }

        System.Console.WriteLine("Visualizers for {0} not found!", data.GetType().Name);
        System.Console.WriteLine();
    }
}