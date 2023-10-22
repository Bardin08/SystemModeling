using SystemModeling.Lab1.Generators;
using SystemModeling.Lab1.Generators.Options;
using SystemModeling.Lab1.Visualization;
using SystemModeling.Lab1.Visualization.Options;

var data = await new ExponentialDistributionGenerator(new ExponentialDistributionOptions()
{
    Amount = 10000,
    Lambda = 2.2
}).Generate();

var histogramVisualizationOptions = new HistogramVisualizationOptions()
{
    Buckets = 20,
    MinValue = 0,
    MaxValue = 5,
    Mode = VisualizationMode.Vertical,
    MaxCharsPerLine = 15
};

var frequencyTableVisualizationOptions = new FrequencyTableVisualizerOptions()
{
    Buckets = 20
};

var visualizerVertical = new ConsoleVisualizer();
await visualizerVertical.VisualizeHistogramAsync(data, histogramVisualizationOptions);

Console.WriteLine();
Console.WriteLine();
Console.WriteLine();

await visualizerVertical.VisualizeFrequencyTableAsync(data, frequencyTableVisualizationOptions);
