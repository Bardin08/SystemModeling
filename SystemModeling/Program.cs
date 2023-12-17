using SystemModeling.Common;
using SystemModeling.Trumpee;
using SystemModeling.Trumpee.Configuration;

var executionArgs = new Dictionary<string, object>()
{
    { "context", "Trumpee - distributed notifications system imitation" },
    { "simulation_setup", null! },
    { "execution", 0 }
};

var runConfigurations = JsonConvert
    .DeserializeObject<List<FactorExperiment>>(
        File.ReadAllText("Trumpet.factor_experiment.json"));

if (runConfigurations?.Any() is null or false)
{
    Console.WriteLine("Experiments info not passed!@ ");
    return;
}

var iteration = 0;
foreach (var cfg in runConfigurations)
{
    iteration++;
    try
    {
        cfg.Configurations!.Prioritization.ProcessingTimeProvider
            = TrumpeeSimulationOptions.DefaultBackoffStrategy;
        cfg.Configurations!.Validation.ProcessingTimeProvider
            = TrumpeeSimulationOptions.DefaultBackoffStrategy;
        cfg.Configurations!.TemplateFilling.ProcessingTimeProvider
            = TrumpeeSimulationOptions.DefaultBackoffStrategy;
        cfg.Configurations!.TransportHub.ProcessingTimeProvider
            = TrumpeeSimulationOptions.DefaultBackoffStrategy;

        executionArgs["execution"] = iteration;
        executionArgs["simulation_setup"] = cfg;

        var runner = new TrumpeeRunnable();
        await runner.RunAsync(executionArgs);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}