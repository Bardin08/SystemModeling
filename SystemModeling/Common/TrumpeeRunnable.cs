﻿using SystemModeling.Common.Interfaces;
using SystemModeling.Lab2.Fluent;
using SystemModeling.Lab2.Fluent.Interfaces;
using SystemModeling.Trumpee.Configuration;

namespace SystemModeling.Common;

internal class TrumpeeRunnable : IRunnable
{
    private static readonly TrumpeeSimulationOptions SimulationOptions = TrumpeeSimulationOptions.Default;

    public async Task RunAsync(Dictionary<string, object> args)
    {
        var trumpeeModel = SimulationProcessorBuilder.CreateBuilder()
            .Simulate()
            .ForSeconds(SimulationOptions.DurationSeconds)
            .WithEventGenerator(epBuilder =>
            {
                epBuilder.AddDelay = SimulationOptions.EventsGenerator.Delay;
                epBuilder.EventsAmount = SimulationOptions.EventsGenerator.TotalEventsAmount;
            })
            .AndRoutingMap(BuildRoutingMap)
            .Build();

        await trumpeeModel.RunImitationAsync();
    }

    private void BuildRoutingMap(IRoutingMapBuilder builder)
    {
        AddValidation(builder);
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
            .UseSingleConsumer(opt =>
            {
                opt.Alias = "validation_processor_thread";
                opt.ProcessingTime = options.AverageValidationTime;
            });

        builder.AddProcessor("validation_dlq", pb => { pb.SetMaxLength(-1); });

        builder.AddProcessor("validation_failed", pb => { pb.SetMaxLength(-1); });

        builder.AddProcessor("validation_passed", pb => { pb.SetMaxLength(-1); });
    }
}