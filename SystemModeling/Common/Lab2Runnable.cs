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
                            ProcessingTime = TimeSpan.Zero,
                            Alias = "__1",
                            Color = ConsoleColor.Gray
                        },
                        new()
                        {
                            ProcessingTime = TimeSpan.Zero,
                            Alias = "__2",
                            Color = ConsoleColor.Cyan
                        },
                        new()
                        {
                            ProcessingTime = TimeSpan.Zero,
                            Alias = "__3",
                            Color = ConsoleColor.Green
                        },
                        new()
                        {
                            ProcessingTime = TimeSpan.Zero,
                            Alias = "__4",
                            Color = ConsoleColor.Yellow
                        },
                        new()
                        {
                            ProcessingTime = TimeSpan.Zero,
                            Alias = "__5",
                            Color = ConsoleColor.Blue
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