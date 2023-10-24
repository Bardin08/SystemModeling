
using SystemModeling.Lab1.Analytics;
using SystemModeling.Lab1.Visualization.Interfaces;

namespace SystemModeling.Lab1.Visualization.Console;

internal class StatisticsVisualizer : IVisualizer
{
    public Task VisualizeAsync<TInput>(TInput stats1)
    {
        if (stats1 is not StatisticsDto stats)
        {
            System.Console.WriteLine("Stats is null. Something went wrong!");
            return Task.CompletedTask;
        }

        System.Console.WriteLine("Statistics information:");
        System.Console.WriteLine("Mean: {0,6:0.00}", stats.Mean);
        System.Console.WriteLine("Variance: {0,6:0.00}", stats.Variance);
        System.Console.WriteLine("Chi Square: {0,6:0.00}", stats.ChiSquare);
        System.Console.WriteLine("Freedom Degree: {0,6:0.00}", stats.FreedomDegree);

        return Task.CompletedTask;
    }
}