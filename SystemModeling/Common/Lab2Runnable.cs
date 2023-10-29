﻿using SystemModeling.Common.Interfaces;
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
        await new SimulationProcessor(new ImitationProcessorOptions())
            .RunImitationAsync(new ImitationOptions
            {
                ImitationTime = TimeSpan.FromMinutes(10),
            });
    }
}