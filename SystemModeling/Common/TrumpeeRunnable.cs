using SystemModeling.Common.Interfaces;
using SystemModeling.Lab2.Fluent;
using SystemModeling.Lab2.Fluent.Interfaces;
using SystemModeling.Lab2.ImitationCore.Backoffs;
using SystemModeling.Trumpee;
using SystemModeling.Trumpee.Configuration;

namespace SystemModeling.Common;

internal class TrumpeeRunnable : IRunnable
{
    private static TrumpeeSimulationOptions _simulationOptions = TrumpeeSimulationOptions.Default;


    public async Task RunAsync(Dictionary<string, object> args)
    {
        var runSetup = (args["simulation_setup"] as FactorExperiment)!; // SimulationOptions;
        Console.WriteLine($"Experiment {args["execution"]}. Experiment name: {runSetup.Name}");

        _simulationOptions = runSetup.Configurations!;
        var trumpeeModel = SimulationProcessorBuilder.CreateBuilder()
            .Simulate()
            .ForSeconds(_simulationOptions.DurationSeconds)
            .WithEventGenerator(epBuilder =>
            {
                var options = _simulationOptions.EventsGenerator;

                epBuilder.BackoffProvider = new LinearBackoff(new LinearBackoffOptions
                {
                    MinDelay = TimeSpan.FromMilliseconds(10),
                    MaxDelay = TimeSpan.FromMilliseconds(100)
                });
                epBuilder.EventsAmount = options.TotalEventsAmount;
                epBuilder.ProcessorName = options.InitialProcessorName;
            })
            .AndRoutingMap(BuildRoutingMap)
            .Build();


        var resultsCollectors = await trumpeeModel.RunImitationAsync();
        var resultObject = new
        {
            SimulationConfig = runSetup,
            Timestamp = DateTime.Now.ToShortTimeString(),
            Result = resultsCollectors
                .Select(collector => new
                {
                    Processor = collector.Key(),
                    Router = collector.Value!()
                }).ToList()
        };

        await using var sw = File.CreateText($"../../../Experiments/{DateTime.Now.Ticks}.json");
        await sw.WriteAsync(JsonConvert.SerializeObject(resultObject));
        sw.Close();
    }

    private void BuildRoutingMap(IRoutingMapBuilder builder)
    {
        AddTemplateFilling(builder);
        AddValidation(builder);
        AddPrioritization(builder);
        AddTransportHub(builder);
    }

    private void AddTemplateFilling(IRoutingMapBuilder builder)
    {
        var options = _simulationOptions.TemplateFilling;

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
                        ProcessingTimeProvider = options.ProcessingTimeProvider,
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
        var options = _simulationOptions.Validation;

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
                        ProcessingTimeProvider = options.ProcessingTimeProvider,
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

        builder.AddProcessor("validation_passed", pb => { pb.AddTransition("prioritize", 1); })
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

    private void AddPrioritization(IRoutingMapBuilder builder)
    {
        var options = _simulationOptions.Prioritization;

        builder.AddProcessor("prioritize", pb =>
            {
                pb.SetMaxLength(options.MaxQueue);

                pb.AddTransition("prioritize_dlq", options.RoutingFailureChance);
                pb.AddTransition("prioritize_failed", options.ValidationFailureChance);
                pb.AddTransition("prioritize_passed", options.SuccessChance);
            })
            .UseConsumers(opt =>
            {
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "prioritize_processor_thread",
                        ProcessingTimeProvider = options.ProcessingTimeProvider,
                        Color = ConsoleColor.Cyan
                    }
                ];
            });

        builder.AddProcessor("prioritize_dlq", pb => { pb.AddTransition("complete", 1); })
            .UseConsumers(opt =>
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "prioritize_processor_dlq",
                        ProcessingTime = TimeSpan.Zero,
                        Color = ConsoleColor.DarkRed
                    }
                ]);

        builder.AddProcessor("prioritize_failed", pb => { pb.AddTransition("complete", 1); })
            .UseConsumers(opt =>
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "prioritize_processor_failed",
                        ProcessingTime = TimeSpan.Zero,
                        Color = ConsoleColor.Red
                    }
                ]);

        builder.AddProcessor("prioritize_passed", pb => { pb.AddTransition("transport-hub", 1); })
            .UseConsumers(opt => opt.ProcessorOptions =
            [
                new ImitationProcessorOptions
                {
                    Alias = "prioritize_complete",
                    ProcessingTime = TimeSpan.Zero,
                    Color = ConsoleColor.Yellow
                }
            ]);
    }

    private void AddTransportHub(IRoutingMapBuilder builder)
    {
        var options = _simulationOptions.TransportHub;

        builder.AddProcessor("transport-hub", pb =>
            {
                pb.SetMaxLength(options.MaxQueue);

                pb.AddTransition("transport-hub_dlq", options.RoutingFailureChance);
                pb.AddTransition("transport-hub_failed", options.ValidationFailureChance);
                pb.AddTransition("transport-hub_passed", options.SuccessChance);
            })
            .UseConsumers(opt =>
            {
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "transport-hub_processor_thread",
                        ProcessingTimeProvider = options.ProcessingTimeProvider,
                        Color = ConsoleColor.Cyan
                    }
                ];
            });

        builder.AddProcessor("transport-hub_dlq", pb => { pb.AddTransition("complete", 1); })
            .UseConsumers(opt =>
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "transport-hub_processor_dlq",
                        ProcessingTime = TimeSpan.Zero,
                        Color = ConsoleColor.DarkRed
                    }
                ]);

        builder.AddProcessor("transport-hub_failed", pb => { pb.AddTransition("complete", 1); })
            .UseConsumers(opt =>
                opt.ProcessorOptions =
                [
                    new ImitationProcessorOptions
                    {
                        Alias = "transport-hub_processor_failed",
                        ProcessingTime = TimeSpan.Zero,
                        Color = ConsoleColor.Red
                    }
                ]);

        builder.AddProcessor("transport-hub_passed", pb => { pb.AddTransition("complete", 1); })
            .UseConsumers(opt => opt.ProcessorOptions =
            [
                new ImitationProcessorOptions
                {
                    Alias = "transport-hub_complete",
                    ProcessingTime = TimeSpan.Zero,
                    Color = ConsoleColor.Yellow
                }
            ]);
    }
}