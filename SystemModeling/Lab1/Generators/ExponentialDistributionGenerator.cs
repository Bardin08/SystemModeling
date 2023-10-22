using SystemModeling.Lab1.Generators.Options;
using SystemModeling.Lab1.Interfaces;

namespace SystemModeling.Lab1.Generators;

internal class ExponentialDistributionGenerator : IGenerator
{
    private static readonly ExponentialDistributionOptions Default = new()
    {
        Amount = 1,
        Lambda = 1
    };

    private readonly ExponentialDistributionOptions _options;

    public ExponentialDistributionGenerator(ExponentialDistributionOptions? options = null)
    {
        _options = options ?? Default;
    }

    public ValueTask<double[]> Generate()
    {
        var nums = new double[_options.Amount];

        for (var i = 0; i < _options.Amount; i++)
        {
            var r = Random.Shared.NextDouble();
            var r1 = -1 * (Math.Log(r) / _options.Lambda);
            nums[i] = r1;
        }

        return ValueTask.FromResult(nums);
    }
}