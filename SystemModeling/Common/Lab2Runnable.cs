using SystemModeling.Common.Interfaces;
using SystemModeling.Lab2.Fluent;
using SystemModeling.Lab2.Options;

namespace SystemModeling.Common;

public class Lab2Runnable : IRunnable
{
    public Task RunAsync(Dictionary<string, object> args)
    {
        SimulationProcessorBuilder
            .CreateBuilder()
            .Simulate()
            .ForSeconds(300)
            .AndRoutingMap(builder =>
            {
                builder.AddProcessor("processor_1", pb => { pb.AddTransition("processor_2", 1); })
                    .UseMultipleConsumers(opt =>
                    {
                        opt.ConsumersAmount = 2;
                        opt.ProcessorOptions = new List<ImitationProcessorOptions>()
                        {
                            new()
                            {
                                Alias = "__1_1",
                                Color = ConsoleColor.Blue,
                                ProcessingTime = TimeSpan.FromSeconds(1)
                            },
                            new()
                            {
                                Alias = "__1_2",
                                Color = ConsoleColor.Yellow,
                                ProcessingTime = TimeSpan.FromSeconds(1)
                            }
                        };
                    });

                builder.AddProcessor("processor_2", pb =>
                    {
                        pb.AddTransition("processor2", 0.1);
                        pb.AddTransition("processor_3", 0.9);
                    })
                    .UseSingleConsumer(opt =>
                    {
                        opt.Alias = "__2_1";
                        opt.ProcessingTime = TimeSpan.FromSeconds(1);
                        opt.Color = ConsoleColor.Green;
                    });

                builder.AddProcessor("processor_3", bp => { bp.AddTransition("complete", 1); })
                    .UseSingleConsumer(opt =>
                    {
                        opt.Alias = "__3_1";
                        opt.Color = ConsoleColor.Red;
                        opt.ProcessingTime = TimeSpan.FromSeconds(1);
                    });
            })
            .Build();
        return Task.CompletedTask;
    }
}