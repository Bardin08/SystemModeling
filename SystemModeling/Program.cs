using SystemModeling.Lab1.Generators;

Console.WriteLine("Exponential: {0}",
    string.Join(" ", await new ExponentialDistributionGenerator().Generate()));
Console.WriteLine("Uniform: {0}",
    string.Join(" ", await new UniformDistributionGenerator().Generate()));
Console.WriteLine("Normal_Randomization: {0}",
    string.Join(" ", await new NormalDistributionGenerator().Generate()));