using System.Threading.Channels;
using SystemModeling.Lab2.ImitationCore.Events;
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
            _cancellationTokenSource.Token);
    }

    public async Task RunImitationAsync()
    {
        _cancellationTokenSource.CancelAfter(_options.SimulationTimeSeconds);

        ArgumentNullException.ThrowIfNull(_options.ProcessorDescriptors);
        foreach (var descriptor in _options.ProcessorDescriptors)
        {
            var processorId = _threadsManager.AddImitationProcessor(descriptor.Value);
            var processorNode = _options.RoutingMap!.FirstOrDefault(n => n.Name == descriptor.Key);

            ArgumentNullException.ThrowIfNull(processorNode);
            processorNode.RouteId = processorId.ToString();
        }

        await _threadsManager.RunAllAsync();
        await Task.CompletedTask;
    }
}