using SystemModeling.Common.Interfaces;
using SystemModeling.Lab2.Fluent;
using SystemModeling.Lab2.Options;

namespace SystemModeling.Common;

public class Lab2Runnable : IRunnable
{
    public Task RunAsync(Dictionary<string, object> args)
    {
        var simulationProcessor = SimulationProcessorBuilder
            .CreateBuilder()
            .Simulate()
            .ForSeconds(300)
            .WithEventGenerator(b =>
            {
                b.EventsAmount = 5;
                b.AddDelay = TimeSpan.FromSeconds(0.5);
            })
            .AndRoutingMap(builder =>
            {
                builder.AddProcessor("processor_1", pb => { pb.AddTransition("processor_2", 1); })
                    .UseMultipleConsumers(opt =>
                    {
                        opt.ConsumersAmount = 2;
                        opt.ProcessorOptions = new List<ImitationProcessorOptions>
                        {
                            new()
                            {
                                Alias = "__1_1",
                                Color = ConsoleColor.Blue,
                                ProcessingTime = TimeSpan.FromSeconds(0.1)
                            },
                            new()
                            {
                                Alias = "__1_2",
                                Color = ConsoleColor.Blue,
                                ProcessingTime = TimeSpan.FromSeconds(0.1)
                            }
                        };
                    });

                builder.AddProcessor("processor_2", pb =>
                    {
                        pb.AddTransition("processor_3", 1);
                    })
                    .UseMultipleConsumers(opt =>
                    {
                        opt.ConsumersAmount = 1;
                        opt.ProcessorOptions = new List<ImitationProcessorOptions>
                        {
                            new()
                            {
                                Alias = "__2_1",
                                ProcessingTime = TimeSpan.FromSeconds(0.05),
                                Color = ConsoleColor.Yellow
                            }
                        };
                    });

                builder.AddProcessor("processor_3", bp => { bp.AddTransition("complete", 1); })
                    .UseMultipleConsumers(opt =>
                    {
                        opt.ConsumersAmount = 1;
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