using SystemModeling.Lab1.Analytics;
using SystemModeling.Lab1.Analytics.Collectors.Options;
using SystemModeling.Lab1.Generators;
using SystemModeling.Lab1.Generators.Options;

var data = await new NormalDistributionGenerator(new NormalDistributionOptions()
{
    Amount = 10000,
    A = 6,
    Sigma = 6
}).Generate();

var builder = new AnalyticsProcessorBuilder()
    .BuildFrequencyMap(new FrequencyMapOptions() { Buckets = 20 })
    .CalculateChiSquare(new DataFilteringOptions() { Threshold = 20 })
    .CalculateMeanAndVariance()
    .BuildProcessor();

var stats = await builder.AnalyzeAsync(data);
if (stats is null)
{
    Console.WriteLine("Statistics collectionning failed");
    return;
}

Console.WriteLine();

Console.WriteLine("Statistics information:");
Console.WriteLine("Mean: {0,6:0.00}", stats.Mean);
Console.WriteLine("Variance: {0,6:0.00}", stats.Variance);


// var r = new ChiSquareMetricCollector(new DataFilteringOptions { Threshold = 20 })
//     .GetMetricAsync(data);

// var stats = await new StatisticsProvider()
//     .GetDatasetStatisticsAsync(data);
// Console.WriteLine("Statistics information:");
// Console.WriteLine("Mean: {0,6:0.00}", stats.Mean);
// Console.WriteLine("Variance: {0,6:0.00}", stats.Variance);
//
// var visualizerVertical = new ConsoleVisualizer();
// Console.WriteLine("Frequency Table:");
//
// var frequencyTableVisualizationOptions = new FrequencyTableVisualizerOptions()
// {
//     Buckets = 10
// };
// await visualizerVertical.VisualizeFrequencyTableAsync(data, frequencyTableVisualizationOptions);
//
// Console.WriteLine("Histogram:");
// var histogramVisualizationOptions = new HistogramVisualizationOptions()
// {
//     Buckets = 10,
//     MinValue = 0,
//     MaxValue = 2,
//     Mode = VisualizationMode.Vertical,
//     MaxCharsPerLine = 15
// };
// await visualizerVertical.VisualizeHistogramAsync(data, histogramVisualizationOptions);