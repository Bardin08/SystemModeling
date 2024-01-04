using SystemModeling.Common.Interfaces;
using SystemModeling.Lab2.Fluent;
using SystemModeling.Lab2.ImitationCore.Backoffs;
using SystemModeling.Lab2.Options.Backoffs;

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
                epBuilder.BackoffProvider = new NormalBackoff(
                    new NormalBackoffOptions(
                        TimeSpan.FromMilliseconds(10),
                        TimeSpan.FromMilliseconds(100)));

                epBuilder.EventsAmount = 100;
                epBuilder.ProcessorName = "init";
            })
            .AndRoutingMap(rb =>
            {
                rb.AddProcessor("init", pb => { pb.AddTransition("complete", 1); })
                    .UseConsumers(cb =>
                    {
                        // TODO: move to fluent API
                        var processingTimeOptions = new NormalBackoffOptions(
                            TimeSpan.Zero,
                            TimeSpan.FromMilliseconds(2));

                        cb.ProcessorOptions =
                        [
                            new ImitationProcessorOptions
                            {
                                ProcessingTimeProvider = new NormalBackoff(processingTimeOptions),
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