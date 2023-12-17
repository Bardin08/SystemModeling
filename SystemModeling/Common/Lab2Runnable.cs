using SystemModeling.Common.Interfaces;
using SystemModeling.Lab2.Fluent;
using SystemModeling.Lab2.ImitationCore.Backoffs;

namespace SystemModeling.Common;

public class Lab2Runnable : IRunnable
{
    public Task RunAsync(Dictionary<string, object> args)
    {
        var simulationProcessor = SimulationProcessorBuilder
            .CreateBuilder()
            .Simulate()
            .ForSeconds(10)
            .WithEventGenerator(b =>
            {
                b.BackoffProvider = new LinearBackoff(new LinearBackoffOptions
                {
                    MinDelay = TimeSpan.FromMilliseconds(10),
                    MaxDelay = TimeSpan.FromMilliseconds(100)
                });
                b.EventsAmount = 25;
                b.ProcessorName = "processor_1";
            })
            .AndRoutingMap(builder =>
            {
                builder.AddProcessor("processor_1", pb =>
                    {
                        pb.SetMaxLength(1);
                        pb.AddTransition("processor_2", 1);
                    })
                    .UseConsumers(opt =>
                    {
                        opt.ProcessorOptions = new List<ImitationProcessorOptions>
                        {
                            new()
                            {
                                Alias = "__1_1",
                                Color = ConsoleColor.Blue,
                                ProcessingTime = TimeSpan.FromSeconds(0.2)
                            },
                            new()
                            {
                                Alias = "__1_2",
                                Color = ConsoleColor.Blue,
                                ProcessingTime = TimeSpan.FromSeconds(0.4)
                            }
                        };
                    });

                builder.AddProcessor("processor_2", pb => { pb.AddTransition("processor_3", 1); })
                    .UseConsumers(opt =>
                    {
                        opt.ProcessorOptions = new List<ImitationProcessorOptions>
                        {
                            new()
                            {
                                Alias = "__2_1",
                                ProcessingTime = TimeSpan.FromSeconds(0.5),
                                Color = ConsoleColor.Yellow
                            }
                        };
                    });

                builder.AddProcessor("processor_3", bp => { bp.AddTransition("complete", 1); })
                    .UseConsumers(opt =>
                    {
                        opt.ProcessorOptions = new List<ImitationProcessorOptions>
                        {
                            new()
                            {
                                Alias = "__3_1",
                                ProcessingTime = TimeSpan.FromSeconds(0.1),
                                Color = ConsoleColor.Green
                            }
                        };
                    });
            })
            .Build();

        return simulationProcessor.RunImitationAsync();
    }
}