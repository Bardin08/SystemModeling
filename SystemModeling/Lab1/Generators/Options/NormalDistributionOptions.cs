namespace SystemModeling.Lab1.Generators.Options;

internal record NormalDistributionOptions : GeneratorOptionsBase
{
    public double Sigma { get; set; }
    public double A { get; set; }
}