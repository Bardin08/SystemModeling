namespace SystemModeling.Lab1.Generators.Options;

internal record UniformDistributionOptions : GeneratorOptionsBase
{
    public long A { get; set; }
    public long C { get; set; }
}