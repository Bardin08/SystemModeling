using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.ImitationCore.Models;

namespace SystemModeling.Lab2.ImitationCore.Observers;

internal class EventProcessorStateObserver : IEventProcessorStateObserver
{
    private readonly object _lockObj = new();

    private readonly Guid _processorId;
    private readonly string _processorName;

    private int _totalQueueSize;
    private int _queueSizeObservations;
    private double _totalLoadTime;
    private int _loadTimeObservations;

    public EventProcessorStateObserver(Guid processorId, string processorName)
    {
        _processorId = processorId;
        _processorName = processorName;
    }

    public void Handle(IObservable observable)
    {
        var processor = observable as IProcessor;
        ArgumentNullException.ThrowIfNull(processor);

        UpdateMetrics(processor.QueueSize, processor.ProcessingTime);
    }

    private void UpdateMetrics(int queueSize, TimeSpan processingTime)
    {
        lock (_lockObj)
        {
            _totalQueueSize += queueSize;
            _queueSizeObservations++;

            _totalLoadTime += processingTime.TotalMilliseconds;
            _loadTimeObservations++;
        }
    }

    public ProcessorStatisticsDto GetProcessorStatistics()
    {
        return new ProcessorStatisticsDto
        {
            ProcessorId = _processorId,
            ProcessorName = _processorName,
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