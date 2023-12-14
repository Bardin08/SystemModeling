using SystemModeling.Common.Interfaces;
using SystemModeling.Lab2.Fluent;
using SystemModeling.Lab2.ImitationCore.Backoffs;

namespace SystemModeling.Common;

public class FancyFeaturesRunnable : IRunnable
{
    public Task RunAsync(Dictionary<string, object> args)
    {
        var trumpeeModel = SimulationProcessorBuilder.CreateBuilder()
            .Simulate()
            .ForSeconds(300)
            .WithEventGenerator(epBuilder =>
            {
                epBuilder.AddDelay = TimeSpan.FromSeconds(0.5);
                epBuilder.EventsAmount = 10;
                epBuilder.ProcessorName = "init";
            })
            .AndRoutingMap(rb =>
            {
                rb.AddProcessor("init", pb => { pb.AddTransition("complete", 1); })
                    .UseConsumers(cb =>
                    {
                        // TODO: move to fluent API
                        var processingTimeOptions = new LinearBackoffOptions
                        {
                            MinDelay = TimeSpan.FromSeconds(0.1),
                            MaxDelay = TimeSpan.FromSeconds(1)
                        };

                        cb.ProcessorOptions =
                        [
                            new ImitationProcessorOptions
                            {
                                ProcessingTimeProvider = new LinearBackoff(processingTimeOptions),
                                Alias = "init_thread",
                                Color = ConsoleColor.Magenta
                            }
                        ];
                    });
            })
            .Build();
        return trumpeeModel.RunImitationAsync();
    }
}