using SystemModeling.Lab1.Generators;
using SystemModeling.Lab1.Generators.Options;
using SystemModeling.Lab1.Visualization;
using SystemModeling.Lab1.Visualization.Options;

var data = await new UniformDistributionGenerator(
    new UniformDistributionOptions()
    {
        Amount = 10000,
        A = 5 ^ 13,
        C = 2 ^ 31,
    }).Generate();

var visualizationOptions = new ConsoleVisualizationOptions()
{
    Buckets = 20,
    MinValue = 0,
    MaxValue = 5,
    Mode = VisualizationMode.Vertical,
    MaxCharsPerLine = 15
};

var visualizerVertical = new ConsoleVisualizer(visualizationOptions);
await visualizerVertical.VisualizeAsync(data);

Console.WriteLine();
Console.WriteLine();
Console.WriteLine();

var visualizerHorizontal = new ConsoleVisualizer(visualizationOptions with { Mode = VisualizationMode.Horizontal });
await visualizerHorizontal.VisualizeAsync(data); 
