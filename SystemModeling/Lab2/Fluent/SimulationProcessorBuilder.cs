using SystemModeling.Lab2.Fluent.Interfaces;
using SystemModeling.Lab2.Options;

namespace SystemModeling.Lab2.Fluent;

internal class SimulationProcessorBuilder :
    ISimulationProcessorBuilder,
    ISimulationOptionsBuilder,
    IRoutingMapBuilderStage
{
    private readonly SimulationOptions _simulationOptions;
    private RoutingMapBuilder? _routingMapBuilder;

    private SimulationProcessorBuilder()
    {
        _simulationOptions = new SimulationOptions();
    }

    public static ISimulationProcessorBuilder CreateBuilder()
    {
        return new SimulationProcessorBuilder();
    }

    public ISimulationOptionsBuilder Simulate()
    {
        return this;
    }

    public IRoutingMapBuilderStage ForSeconds(int seconds)
    {
        _simulationOptions.SimulationTimeSeconds = TimeSpan.FromSeconds(seconds);
        return this;
    }

    public SimulationProcessorBuilder WithEventGenerator(Action<EventProviderOptions> builder)
    {
        var eventsProviderOptions = new EventProviderOptions();
        builder(eventsProviderOptions);
        _simulationOptions.EventProviderOptions = eventsProviderOptions;
        return this;
    }

    public SimulationProcessorBuilder AndRoutingMap(Action<IRoutingMapBuilder> builder)
    {
        _routingMapBuilder = RoutingMapBuilder
            .CreateBuilder(_simulationOptions.ProcessorDescriptors!);
        builder(_routingMapBuilder);
        _simulationOptions.RoutingMap = _routingMapBuilder.Build();
        return this;
    }

    public SimulationProcessor Build()
    {
        var simulationProcessor = new SimulationProcessor(_simulationOptions);
        return simulationProcessor;
    }
}