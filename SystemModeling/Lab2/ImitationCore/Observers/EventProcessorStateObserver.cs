using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.ImitationCore.Models;

namespace SystemModeling.Lab2.ImitationCore.Observers;

internal class EventProcessorStateObserver : IEventProcessorStateObserver
{
    private readonly object _lockObj = new();

    private readonly Guid _processorId;

    private int _totalQueueSize;
    private int _queueSizeObservations;
    private double _totalLoadTime;
    private int _loadTimeObservations;

    public EventProcessorStateObserver(Guid processorId)
    {
        _processorId = processorId;
    }

    public void Handle(IObservable observable)
    {
        lock (_lockObj)
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
    }

    private void UpdateMetrics(int queueSize, TimeSpan processingTime)
    {
        _totalQueueSize += queueSize;
        _queueSizeObservations++;

        _totalLoadTime += processingTime.TotalMilliseconds;
        _loadTimeObservations++;
    }

    public ProcessorStatisticsDto? GetProcessorStatistics()
    {
        return new ProcessorStatisticsDto
        {
            ProcessorId = _processorId,
            LoadTimeObservations = _loadTimeObservations,
            TotalLoadTime = _totalLoadTime,
            TotalQueueSize = _totalQueueSize,
            QueueSizeObservations = _queueSizeObservations,
            MeadQueueLength = MeanQueueLength,
            MeanLoadTime = MeanLoadTime
        };
    }

    private double MeanQueueLength => _queueSizeObservations > 0
        ? (double)_totalQueueSize / _queueSizeObservations
        : 0;

    private double MeanLoadTime => _loadTimeObservations > 0
        ? _totalLoadTime / _loadTimeObservations
        : 0;
}