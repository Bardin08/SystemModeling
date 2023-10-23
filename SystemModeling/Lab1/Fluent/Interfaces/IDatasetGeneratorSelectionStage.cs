using SystemModeling.Lab1.Generators.Options;

namespace SystemModeling.Lab1.Fluent.Interfaces;

internal interface IDatasetGeneratorSelectionStage
{
    IProcessorBuilder WithExponentialDistribution(
        Action<ExponentialDistributionOptions>? factory = null);

    IProcessorBuilder WithNormalDistribution(
        Action<NormalDistributionOptions>? factory = null);

    IProcessorBuilder WithUniformDistribution(
        Action<UniformDistributionOptions>? factory = null);
}