using SystemModeling.Lab1.Analytics.Collectors;
using SystemModeling.Lab1.Visualization.Interfaces;

namespace SystemModeling.Lab1.Visualization.Console;

internal class FrequencyMapVisualizer : IVisualizer
{
    public Task VisualizeAsync<TInput>(TInput input)
    {
        var frequencyMap = (input as SortedSet<FrequencyMapBucket>)!;

        foreach (var bucket in frequencyMap)
        {
            System.Console.Write($"{bucket.Min,6:0.0} - {bucket.Max,6:0.0} : {bucket.ItemsAmount} ");
            System.Console.WriteLine();
        }

        return Task.CompletedTask;
    }
}