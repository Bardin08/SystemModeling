using SystemModeling.Common.Interfaces;
using SystemModeling.Lab2;
using SystemModeling.Lab2.Options;

namespace SystemModeling.Common;

public class Lab2Runnable : IRunnable
{
    public async Task RunAsync(Dictionary<string, object> args)
    {
        Console.WriteLine("Execution context: {0}", args["context"]);

        await RunInternalAsync();
    }

    private async Task RunInternalAsync()
    {
        await new SimulationProcessor(new SimulationOptions
            {
                ImitationProcessorFactoryOptions = new MultiConsumersImitationProcessorOptions
                {
                    ConsumersAmount = 5,
                    ProcessorOptions = new List<ImitationProcessorOptions>
                    {
                        new()
                        {
                            ProcessingTime = TimeSpan.FromSeconds(1),
                        },
                        new()
                        {
                            ProcessingTime = TimeSpan.FromSeconds(1.1),
                        },
                        new()
                        {
                            ProcessingTime = TimeSpan.FromSeconds(1.2),
                        },
                        new()
                        {
                            ProcessingTime = TimeSpan.FromSeconds(1.3),
                        },
                        new()
                        {
                            ProcessingTime = TimeSpan.FromSeconds(1.4),
                        },
                    }
                }
            })
            .RunImitationAsync(new ImitationOptions
            {
                ImitationTime = TimeSpan.FromMinutes(10),
            });
    }
}