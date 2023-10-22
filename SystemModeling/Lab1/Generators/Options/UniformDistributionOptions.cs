namespace SystemModeling.Lab1.Generators.Options;

internal record UniformDistributionOptions : GeneratorOptionsBase
{
    public long A { get; init; }
    public long C { get; init; }
}