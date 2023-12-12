using SystemModeling.Common.Interfaces;
using SystemModeling.Lab2.Fluent;
using SystemModeling.Lab2.Fluent.Interfaces;
using SystemModeling.Lab2.Options;
using SystemModeling.Trumpee.Configuration;

namespace SystemModeling.Common;

internal class TrumpeeRunnable : IRunnable
{
    private static readonly TrumpeeSimulationOptions SimulationOptions = TrumpeeSimulationOptions.Default;

    public Task RunAsync(Dictionary<string, object> args)
    {
        var trumpeeModel = SimulationProcessorBuilder.CreateBuilder()
            .Simulate()
            .ForSeconds(SimulationOptions.DurationSeconds)
            .WithEventGenerator(epBuilder =>
            {
                epBuilder.AddDelay = SimulationOptions.EventsGenerator.Delay;
                epBuilder.EventsAmount = SimulationOptions.EventsGenerator.TotalEventsAmount;
                epBuilder.ProcessorName = SimulationOptions.EventsGenerator.InitialProcessorName;
            })
            .AndRoutingMap(BuildRoutingMap)
            .Build();

        return trumpeeModel.RunImitationAsync();
    }

    private void BuildRoutingMap(IRoutingMapBuilder builder)
    {
        AddTemplateFilling(builder);
        AddValidation(builder);
    }

    private void AddTemplateFilling(IRoutingMapBuilder builder)
    {
        var options = SimulationOptions.Validation;

        builder.AddProcessor("template-filling", pb =>
            {
                pb.SetMaxLength(options.MaxQueue);

                pb.AddTransition("template-filling_dlq", options.RoutingFailureChance);
                pb.AddTransition("template-filling_failed", options.ValidationFailureChance);
                pb.AddTransition("template-filling_passed", options.SuccessChance);
            })
            .UseConsumers(opt =>
            {
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "template-filling_processor_thread",
                        ProcessingTime = options.AverageValidationTime,
                        Color = ConsoleColor.Cyan
                    }
                ];
            });

        builder.AddProcessor("template-filling_dlq", pb => { pb.AddTransition("complete", 1); })
            .UseConsumers(opt =>
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "template-filling_processor_dlq",
                        ProcessingTime = TimeSpan.Zero,
                        Color = ConsoleColor.DarkRed
                    }
                ]);

        builder.AddProcessor("template-filling_failed", pb => { pb.AddTransition("complete", 1); })
            .UseConsumers(opt =>
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "template-filling_processor_failed",
                        ProcessingTime = TimeSpan.Zero,
                        Color = ConsoleColor.Red
                    }
                ]);

        builder.AddProcessor("template-filling_passed", pb => { pb.AddTransition("validation", 1); })
            .UseConsumers(opt => opt.ProcessorOptions =
            [
                new ImitationProcessorOptions
                {
                    Alias = "template-filling_complete",
                    ProcessingTime = TimeSpan.Zero,
                    Color = ConsoleColor.Yellow
                }
            ]);
    }

    private void AddValidation(IRoutingMapBuilder builder)
    {
        var options = SimulationOptions.Validation;

        builder.AddProcessor("validation", pb =>
            {
                pb.SetMaxLength(options.MaxQueue);

                pb.AddTransition("validation_dlq", options.RoutingFailureChance);
                pb.AddTransition("validation_failed", options.ValidationFailureChance);
                pb.AddTransition("validation_passed", options.SuccessChance);
            })
            .UseConsumers(opt =>
            {
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "validation_processor_thread",
                        ProcessingTime = options.AverageValidationTime,
                        Color = ConsoleColor.DarkMagenta
                    }
                ];
            });

        builder.AddProcessor("validation_dlq", pb => { pb.AddTransition("complete", 1); })
            .UseConsumers(opt =>
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "validation_processor_dlq",
                        ProcessingTime = TimeSpan.Zero,
                        Color = ConsoleColor.DarkRed
                    }
                ]);

        builder.AddProcessor("validation_failed", pb => { pb.AddTransition("complete", 1); })
            .UseConsumers(opt =>
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "validation_processor_failed",
                        ProcessingTime = TimeSpan.Zero,
                        Color = ConsoleColor.Red
                    }
                ]);

        builder.AddProcessor("validation_passed", pb => { pb.AddTransition("complete", 1); })
            .UseConsumers(opt => opt.ProcessorOptions =
            [
                new ImitationProcessorOptions
                {
                    Alias = "validation_complete",
                    ProcessingTime = TimeSpan.Zero,
                    Color = ConsoleColor.Yellow
                }
            ]);
    }
}