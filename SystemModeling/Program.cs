using SystemModeling.Lab1.Analytics;
using SystemModeling.Lab1.Generators;
using SystemModeling.Lab1.Generators.Options;
using SystemModeling.Lab1.Visualization;
using SystemModeling.Lab1.Visualization.Options;

var data = await new ExponentialDistributionGenerator(new ExponentialDistributionOptions()
{
    Amount = 10000,
    Lambda = 2.2
}).Generate();


var stats = await new StatisticsProvider()
    .GetDatasetStatisticsAsync(data);
Console.WriteLine("Statistics information:");
Console.WriteLine("Mean: {0,6:0.00}", stats.Mean);
Console.WriteLine("Variance: {0,6:0.00}", stats.Variance);

var visualizerVertical = new ConsoleVisualizer();
Console.WriteLine("Frequency Table:");

var frequencyTableVisualizationOptions = new FrequencyTableVisualizerOptions()
{
    Buckets = 10
};
await visualizerVertical.VisualizeFrequencyTableAsync(data, frequencyTableVisualizationOptions);

Console.WriteLine("Histogram:");
var histogramVisualizationOptions = new HistogramVisualizationOptions()
{
    Buckets = 10,
    MinValue = 0,
    MaxValue = 2,
    Mode = VisualizationMode.Vertical,
    MaxCharsPerLine = 15
};
await visualizerVertical.VisualizeHistogramAsync(data, histogramVisualizationOptions);