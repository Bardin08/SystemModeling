using SystemModeling.Lab1.Analytics.Collectors.Options;
using SystemModeling.Lab1.Fluent;
using SystemModeling.Lab1.Visualization.Options;

var processor = FluentProcessorBuilder
    .CreateBuilder()
    .Generate()
    .WithExponentialDistribution(opt =>
    {
        opt.Lambda = .5;
        opt.Amount = 10000;
    })
    .Analyze()
    .AddCollectors(ab =>
    {
        ab.BuildFrequencyMap(new FrequencyMapOptions { Buckets = 15 })
            .CalculateMeanAndVariance()
            .CalculateChiSquare(new DataFilteringOptions { Threshold = 3 });
    })
    .Visualize()
    .AddVisualizers(vb =>
    {
        vb.AddStatisticsVisualization();
        vb.AddFrequencyMapVisualization();
        vb.AddHistogramVisualization(opt =>
        {
            opt.Buckets = 15;
            opt.MinValue = 0;
            opt.MaxValue = 5;
            opt.MaxCharsPerLine = 15;
            opt.Mode = VisualizationMode.Horizontal;
        });
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
