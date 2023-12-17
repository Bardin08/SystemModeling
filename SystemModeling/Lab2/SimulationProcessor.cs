using SystemModeling.Lab2.ImitationCore.Events;
using SystemModeling.Lab2.ImitationCore.Models;
using SystemModeling.Lab2.ImitationCore.Observers;
using SystemModeling.Lab2.Options;
using SystemModeling.Lab2.Routing.Models;
using SystemModeling.Lab2.Routing.Services;

namespace SystemModeling.Lab2;

internal sealed class SimulationProcessor
{
    private readonly SimulationOptions _options;
    private readonly TasksManager<string> _threadsManager;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public SimulationProcessor(SimulationOptions options)
    {
        _options = options;

        _cancellationTokenSource = new CancellationTokenSource();

        _threadsManager = new TasksManager<string>(
            new RoutingMapService(_options.RoutingMap!),
            new StringEventsProvider(_options.EventProviderOptions),
            Channel.CreateUnbounded<EventContext<string>>(),
            _cancellationTokenSource);
    }

    public async Task<Dictionary<Func<ProcessorStatisticsDto>, Func<ProcessorRoutingDescriptor>?>> RunImitationAsync()
    {
        var collectors = new Dictionary<Func<ProcessorStatisticsDto>, Func<ProcessorRoutingDescriptor>?>();

        try
        {
            _cancellationTokenSource.CancelAfter(_options.SimulationTimeSeconds);

            ArgumentNullException.ThrowIfNull(_options.ProcessorDescriptors);
            foreach (var descriptor in _options.ProcessorDescriptors)
            {
                var processorNode = _options.RoutingMap!
                    .FirstOrDefault(n => n.Name == descriptor.Key);
                ArgumentNullException.ThrowIfNull(processorNode);

                var processorInfo = _threadsManager
                    .AddImitationProcessor(descriptor.Value, processorNode);

                ArgumentNullException.ThrowIfNull(processorNode);
                processorNode.RouteId = processorInfo.ThreadId.ToString();

                var processorStats = processorInfo.GetProcessorStatsFunc;
                var routerStats = processorInfo.RoutingStatsFunc;

                collectors.Add(processorStats!, routerStats);
            }

            await _threadsManager.RunAllAsync();
        }
        catch
        {
            // ignored
        }

        return collectors;
        // PrintProcessorStatistics(collectors);
    }

    private static string PrintProcessorStatistics(
        Dictionary<Func<ProcessorStatisticsDto>, Func<ProcessorRoutingDescriptor>?> statisticsCollectors)
    {
        if (statisticsCollectors.Count == 0)
        {
            return "";
        }

        var sb = new StringBuilder();
        foreach (var statsInfo in statisticsCollectors)
        {
            var (processorMetrics, routerMetrics) = (statsInfo.Key(), statsInfo.Value!());

            if (routerMetrics.SuccessEvents is 0 || (bool)processorMetrics.ProcessorName?.EndsWith("passed"))
            {
                continue;
            }

            if (processorMetrics is null || routerMetrics is null)
            {
                Console.WriteLine("No info received");
                continue;
            }

            sb.Append($"ThreadId: {processorMetrics.ProcessorId}:{processorMetrics.ProcessorName}").AppendLine()
                .Append($"Mean Queue Size: {processorMetrics.MeadQueueLength}").AppendLine()
                .Append($"Mean Load Time: {processorMetrics.MeanLoadTime}").AppendLine()
                .Append($"Success: {routerMetrics.SuccessEvents}").AppendLine()
                .Append($"Failed: {routerMetrics.FailedEvents}").AppendLine()
                .Append($"Failure Chance: {routerMetrics.MeanFailChance}").AppendLine()
                .AppendLine();
        }

        var str = sb.ToString();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(str);
        Console.ResetColor();

        return str;
    }
}