﻿using SystemModeling.Lab1.Fluent.Interfaces;

namespace SystemModeling.Lab1.Fluent;

internal partial class FluentProcessorBuilder :
    IProcessorBuilder
{
    private FluentProcessorBuilder()
    {
    }

    public static IProcessorBuilder CreateBuilder()
    {
        return new FluentProcessorBuilder();
    }

    public IDatasetGeneratorSelectionStage Generate()
    {
        return this;
    }

    public IAnalyticsStepsSelectionStage Analyze()
    {
        return this;
    }

    public IVisualizersSelectionStage Visualize()
    {
        return this;
    }

    public Processor Build()
    {
        ArgumentNullException.ThrowIfNull(_generator);
        ArgumentNullException.ThrowIfNull(_analyticsProcessor);
        ArgumentNullException.ThrowIfNull(_visualizationProcessor);

        return new Processor(
            _generator,
            _analyticsProcessor,
            _visualizationProcessor);
    }
}