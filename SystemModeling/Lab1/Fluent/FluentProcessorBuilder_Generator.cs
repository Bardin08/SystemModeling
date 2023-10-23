using SystemModeling.Lab1.Fluent.Interfaces;
using SystemModeling.Lab1.Generators;
using SystemModeling.Lab1.Generators.Interfaces;
using SystemModeling.Lab1.Generators.Options;

namespace SystemModeling.Lab1.Fluent;

internal partial class FluentProcessorBuilder : IDatasetGeneratorSelectionStage
{
    private IGenerator? _generator;

    public IProcessorBuilder WithExponentialDistribution(
        Action<ExponentialDistributionOptions>? factory = null)
    {
        var options = new ExponentialDistributionOptions();
        factory?.Invoke(options);

        _generator = new ExponentialDistributionGenerator(options);

        return this;
    }

    public IProcessorBuilder WithNormalDistribution(
        Action<NormalDistributionOptions>? factory = null)
    {
        var options = new NormalDistributionOptions();
        factory?.Invoke(options);

        _generator = new NormalDistributionGenerator(options);

        return this;
    }

    public IProcessorBuilder WithUniformDistribution(
        Action<UniformDistributionOptions>? factory = null)
    {
        var options = new UniformDistributionOptions();
        factory?.Invoke(options);

        _generator = new UniformDistributionGenerator(options);

        return this;
    }
}