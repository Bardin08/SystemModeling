using SystemModeling.Lab1.Analytics.Collectors.Options;
using SystemModeling.Lab1.Fluent;

var processor = FluentProcessorBuilder
    .CreateBuilder()
    .Generate()
    .WithUniformDistribution(opt =>
    {
        opt.A = 5^13;
        opt.C = 2^31;
        opt.Amount = 10000;
    })
    .Analyze()
    .AddCollectors(ab =>
    {
        ab.BuildFrequencyMap(new FrequencyMapOptions { Buckets = 15 })
            .CalculateMeanAndVariance()
            .CalculateChiSquare(new DataFilteringOptions { Threshold = 20 });
    })
    .Build();

await processor.Generate();
var stats = await processor.Analyze();

if (stats is null)
{
    Console.WriteLine("Stats is null. Something went wrong!");
    return;
}

Console.WriteLine("Statistics information:");
Console.WriteLine("Mean: {0,6:0.00}", stats.Mean);
Console.WriteLine("Variance: {0,6:0.00}", stats.Variance);
