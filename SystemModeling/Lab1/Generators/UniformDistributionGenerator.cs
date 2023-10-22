using SystemModeling.Lab1.Generators.Options;
using SystemModeling.Lab1.Interfaces;

namespace SystemModeling.Lab1.Generators;

internal class UniformDistributionGenerator : IGenerator
{
    private static readonly UniformDistributionOptions Default = new()
    {
        Amount = 1,
        Seed = 109,
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

        for (var i = 0; i < _options.Amount; i++)
        {
            var z = (_options.A * _options.Seed) % _options.C;
            var r1 = (double) z / _options.C;
            nums[i] = r1;
        }

        return ValueTask.FromResult(nums);
    }
}