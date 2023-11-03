using SystemModeling.Lab2.ImitationCore.Interfaces;

namespace SystemModeling.Lab2.ImitationCore.Observers;

internal class EventProcessorStateObserver : IObserver
{
    private int _totalQueueSize;
    private int _queueSizeObservations;
    private double _totalLoadTime;
    private int _loadTimeObservations;

    public void Handle(IObservable observable)
    {
        var processor = observable as IProcessor;
        ArgumentNullException.ThrowIfNull(processor);

        UpdateMetrics(processor.QueueSize, processor.ProcessingTime);
        var sb = new StringBuilder();
        sb.Append($"ThreadId: {processor.ProcessorId}. ").AppendLine()
            .Append($"Current Queue Size: {processor.QueueSize}").AppendLine()
            .Append($"Mean Queue Size: {MeanQueueLength}").AppendLine()
            .Append($"Mean Load Time: {MeanLoadTime}").AppendLine();

        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(sb.ToString());
        Console.ResetColor();
    }

    private void UpdateMetrics(int queueSize, TimeSpan processingTime)
    {
        _totalQueueSize += queueSize;
        _queueSizeObservations++;

        _totalLoadTime += processingTime.TotalSeconds;
        _loadTimeObservations++;
    }

    private double MeanQueueLength => _queueSizeObservations > 0
        ? (double)_totalQueueSize / _queueSizeObservations
        : 0;

    private double MeanLoadTime => _loadTimeObservations > 0
        ? _totalLoadTime / _loadTimeObservations
        : 0;
}