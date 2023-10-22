using SystemModeling.Lab1.Interfaces;
using SystemModeling.Lab1.Visualization.Options;

namespace SystemModeling.Lab1.Visualization;

internal class ConsoleVisualizer : IVisualizer
{
    private readonly ConsoleVisualizationOptions _options;

    public ConsoleVisualizer(ConsoleVisualizationOptions options)
    {
        _options = options;
    }

    public ValueTask VisualizeAsync(IEnumerable<double> data)
    {
        var bucketRange = (_options.MaxValue - _options.MinValue) / _options.Buckets;

        var buckets = SplitDataToBuckets(data, bucketRange);

        var maxFrequency = buckets.Prepend(0).Max();

        var scaleFactor = _options.MaxCharsPerLine / (double)maxFrequency;

        switch (_options.Mode)
        {
            case VisualizationMode.Vertical:
                PrintVertical(buckets, scaleFactor, bucketRange);
                break;
            case VisualizationMode.Horizontal:
                PrintHorizontal(buckets, maxFrequency, scaleFactor, bucketRange);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return ValueTask.CompletedTask;
    }

    private int[] SplitDataToBuckets(IEnumerable<double> data, double bucketRange)
    {
        var buckets = new int[_options.Buckets];

        foreach (var value in data)
        {
            var bucketIndex = (int)((value - _options.MinValue) / bucketRange);

            if (bucketIndex >= 0 && bucketIndex < _options.Buckets)
            {
                buckets[bucketIndex]++;
            }
        }

        return buckets;
    }

    private void PrintVertical(int[] buckets, double scaleFactor, double bucketRange)
    {
        for (var i = 0; i < _options.Buckets; i++)
        {
            var bucketMin = _options.MinValue + i * bucketRange;
            var bucketMax = bucketMin + bucketRange;
            Console.Write($"{bucketMin,6:0.0} - {bucketMax,6:0.0} : ");

            var numAsterisks = (int)(buckets[i] * scaleFactor);
            for (var j = 0; j < numAsterisks; j++)
            {
                Console.Write("*");
            }

            Console.WriteLine();
        }
    }

    private void PrintHorizontal(int[] buckets, int maxFrequency,
        double scaleFactor, double bucketRange)
    {
        for (var i = (int)(maxFrequency * scaleFactor); i > 0; i--)
        {
            for (var j = 0; j < _options.Buckets; j++)
            {
                Console.Write("{0,4}", (int)(buckets[j] * scaleFactor) >= i ? "@" : " ");
            }
            Console.WriteLine();
        }

        for (var i = 0; i < _options.Buckets; i++)
        {
            Console.Write("{0,4}", "----");
        }
        Console.WriteLine();

        for (var i = 0; i < _options.Buckets; i++)
        {
            Console.Write("{0,4:0.0}", bucketRange * i);
        }
        Console.WriteLine();
    }
}