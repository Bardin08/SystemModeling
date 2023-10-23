namespace SystemModeling.Lab1.Generators.Options;

internal record ExponentialDistributionOptions : GeneratorOptionsBase
{
    public double Lambda { get; set; }
}