using SystemModeling.Lab1.Generators.Interfaces;
using SystemModeling.Lab1.Generators.Options;

namespace SystemModeling.Lab1.Generators;

internal class UniformDistributionGenerator : IGenerator
{
    private static readonly UniformDistributionOptions Default = new()
    {
        Amount = 1,
        A = 5 ^ 13,
        C = 2 ^ 31
    };
    
    private readonly UniformDistributionOptions _options;

    public UniformDistributionGenerator(UniformDistributionOptions? options = null)
    {
        _options = options ?? Default;
    }

    public ValueTask<double[]> Generate()
    {
        var nums = new double[_options.Amount];
        var z = Random.Shared.NextDouble();
        
        for (var i = 0; i < _options.Amount; i++)
        {
            z = _options.A * z % _options.C;
            var generatedNumber = z / _options.C;
            nums[i] = generatedNumber;
        }

        return ValueTask.FromResult(nums);
    }
}