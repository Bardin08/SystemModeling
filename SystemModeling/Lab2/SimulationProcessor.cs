using SystemModeling.Lab2.ImitationCore.Events;
using SystemModeling.Lab2.ImitationCore.Models;
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

    public async Task RunImitationAsync()
    {
        _cancellationTokenSource.CancelAfter(_options.SimulationTimeSeconds);

        var statisticsCollectors = new List<Func<ProcessorStatisticsDto>?>();

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

            statisticsCollectors.Add(processorInfo.GetProcessorStatsFunc);
        }

        await _threadsManager.RunAllAsync();

        var fetchedStats = statisticsCollectors
            .Where(collector => collector is not null)
            .Select(collector => collector!())
            .ToList();

        if (!fetchedStats.Any()) return;

        var sb = new StringBuilder();
        foreach (var statsInfo in fetchedStats)
        {
            sb.Append($"ThreadId: {statsInfo.ProcessorId}. ").AppendLine()
                .Append($"Mean Queue Size: {statsInfo}").AppendLine()
                .Append($"Mean Load Time: {statsInfo}").AppendLine();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(sb.ToString());
            Console.ResetColor();
        }

        sb.Clear();

        await Task.CompletedTask;
    }
}