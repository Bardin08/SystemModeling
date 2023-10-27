using SystemModeling.Common.Interfaces;
using SystemModeling.Lab1.Analytics.Collectors.Options;
using SystemModeling.Lab1.Fluent;

namespace SystemModeling.Common;

public class Lab1Runnable : IRunnable
{
    public async Task RunAsync(Dictionary<string, object> args)
    {
        Console.WriteLine("Execution context: {0}", args["context"]);

        await RunInternalAsync();
    }

    private async Task RunInternalAsync()
    {
        var processor = FluentProcessorBuilder
            .CreateBuilder()
            .Generate()
            .WithUniformDistribution(opt =>
            {
                opt.Amount = 10000;
                opt.A = 7 ^ 11;
                opt.C = 5 ^ 13;
            })
            .Analyze()
            .AddCollectors(ab =>
            {
                ab.BuildFrequencyMap(new FrequencyMapOptions { Buckets = 100 })
                    .CalculateMeanAndVariance()
                    .CalculateChiSquare(new DataFilteringOptions { Threshold = 3 });
            })
            .Visualize()
            .AddVisualizers(vb =>
            {
                vb.AddStatisticsVisualization();
                vb.AddFrequencyMapVisualization();
                // vb.AddHistogramVisualization(opt =>
                // {
                //     opt.Buckets = 15;
                //     opt.MinValue = 0;
                //     opt.MaxValue = 5;
                //     opt.MaxCharsPerLine = 15;
                //     opt.Mode = VisualizationMode.Horizontal;
                // });
            })
            .Build();

        await processor.Generate();
        var stats = await processor.Analyze();

        if (stats is null)
        {
            Console.WriteLine("Stats is null. Something went wrong!");
            return;
        }

        await processor.Visualize(stats);
        await processor.Visualize(stats.FrequencyMap);
    }
}