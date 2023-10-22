using SystemModeling.Lab1.Visualization.Interfaces;
using SystemModeling.Lab1.Visualization.Options;

namespace SystemModeling.Lab1.Visualization;

internal class ConsoleVisualizer : IHistogramVisualizer, IFrequencyTableVisualizer
{
    public ValueTask VisualizeFrequencyTableAsync(double[] data, FrequencyTableVisualizerOptions options)
    {
        var min = data.Min();
        var max = data.Max();

        var bucketRange = (max - min) / options.Buckets;

        var buckets = SplitDataToBuckets(data, bucketRange, min, options.Buckets);

        for (var i = 0; i < options.Buckets; i++)
        {
            var bucketMin = min + i * bucketRange;
            var bucketMax = bucketMin + bucketRange;
            Console.Write($"{bucketMin,6:0.0} - {bucketMax,6:0.0} : {buckets[i]} ");
            Console.WriteLine();
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask VisualizeHistogramAsync(double[] data, HistogramVisualizationOptions options)
    {
        var bucketRange = (options.MaxValue - options.MinValue) / options.Buckets;

        var buckets = SplitDataToBuckets(data, bucketRange, options.MinValue, options.Buckets);

        var maxFrequency = buckets.Prepend(0).Max();

        var scaleFactor = options.MaxCharsPerLine / (double)maxFrequency;

        switch (options.Mode)
        {
            case VisualizationMode.Vertical:
                PrintVertical(buckets, scaleFactor, bucketRange, options.MinValue);
                break;
            case VisualizationMode.Horizontal:
                PrintHorizontal(buckets, maxFrequency, scaleFactor, bucketRange);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return ValueTask.CompletedTask;
    }

    private int[] SplitDataToBuckets(IEnumerable<double> data,
        double bucketRange, double minValue, int bucketsAmount)
    {
        var buckets = new int[bucketsAmount];

        foreach (var value in data)
        {
            var bucketIndex = (int)((value - minValue) / bucketRange);

            if (bucketIndex >= 0 && bucketIndex < bucketsAmount)
            {
                buckets[bucketIndex]++;
            }
        }

        return buckets;
    }

    private void PrintVertical(int[] buckets, double scaleFactor, double bucketRange, double minValue)
    {
        for (var i = 0; i < buckets.Length; i++)
        {
            var bucketMin = minValue + i * bucketRange;
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
            foreach (var bucket in buckets)
            {
                Console.Write("{0,4}", (int)(bucket * scaleFactor) >= i ? "@" : " ");
            }

            Console.WriteLine();
        }

        for (var i = 0; i < buckets.Length; i++)
        {
            Console.Write("{0,4}", "----");
        }
        Console.WriteLine();

        for (var i = 0; i < buckets.Length; i++)
        {
            Console.Write("{0,4:0.0}", bucketRange * i);
        }
        Console.WriteLine();
    }
}