using SystemModeling.Lab1.Analytics.Collectors;
using SystemModeling.Lab1.Visualization.Interfaces;
using SystemModeling.Lab1.Visualization.Options;

namespace SystemModeling.Lab1.Visualization.Console;

internal class HistogramVisualizer : IVisualizer
{
    private readonly HistogramVisualizationOptions _options;

    public HistogramVisualizer(HistogramVisualizationOptions options)
    {
        _options = options;
    }

    public Task VisualizeAsync<TInput>(TInput input)
    {
        var frequencyMap = (input as SortedSet<FrequencyMapBucket>)!;
        var firstBucket = frequencyMap.First();
        var bucketRange = firstBucket.Max - firstBucket.Min;

        var maxFrequency = frequencyMap.First().ItemsAmount;
        var scaleFactor = _options.MaxCharsPerLine / (double)maxFrequency;

        switch (_options.Mode)
        {
            case VisualizationMode.Vertical:
                PrintVertical(frequencyMap, scaleFactor, bucketRange, _options.MinValue);
                break;
            case VisualizationMode.Horizontal:
                PrintHorizontal(frequencyMap, maxFrequency, scaleFactor, bucketRange);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return Task.CompletedTask;
    }


    private void PrintVertical(SortedSet<FrequencyMapBucket> frequencyMap,
        double scaleFactor, double bucketRange, double minValue)
    {
        var i = 0;
        foreach (var bucket in frequencyMap)
        {
            var bucketMin = minValue + i * bucketRange;
            var bucketMax = bucketMin + bucketRange;
            System.Console.Write($"{bucketMin,6:0.0} - {bucketMax,6:0.0} : ");

            var numAsterisks = (int)(bucket.ItemsAmount * scaleFactor);
            for (var j = 0; j < numAsterisks; j++)
            {
                System.Console.Write("*");
            }

            i++;
        }
    }

    private void PrintHorizontal(SortedSet<FrequencyMapBucket> frequencyMap, int maxFrequency,
        double scaleFactor, double bucketRange)
    {
        for (var i = (int)(maxFrequency * scaleFactor); i > 0; i--)
        {
            foreach (var bucket in frequencyMap)
            {
                System.Console.Write("{0,4}", (int)(bucket.ItemsAmount * scaleFactor) >= i ? "@" : " ");
            }

            System.Console.WriteLine();
        }

        for (var i = 0; i < frequencyMap.Count; i++)
        {
            System.Console.Write("{0,4}", "----");
        }

        System.Console.WriteLine();

        for (var i = 0; i < frequencyMap.Count; i++)
        {
            System.Console.Write("{0,4:0.0}", bucketRange * i);
        }

        System.Console.WriteLine();
    }
}