using System.Runtime.CompilerServices;
using SystemModeling.Lab1.Generators.Options;
using SystemModeling.Lab1.Interfaces;

namespace SystemModeling.Lab1.Generators;

internal class NormalDistributionGenerator : IGenerator
{
    private static readonly NormalDistributionOptions Default = new()
    {
        Amount = 1,
        Sigma = 1,
        A = 0
    };

    private readonly NormalDistributionOptions _options;

    public NormalDistributionGenerator(NormalDistributionOptions? options = null)
    {
        _options = options ?? Default;
    }

    public ValueTask<double[]> Generate()
    {
        var nums = new double[_options.Amount];

        for (var i = 0; i < _options.Amount; i++)
        {
            const int sumNumsAmount = 12;

            var s = GetSum(sumNumsAmount);

            var m = s - 6;
            var generatedNum = _options.Sigma * m + _options.A;

            nums[i] = generatedNum;
        }

        return ValueTask.FromResult(nums);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private double GetSum(int sumNumsAmount)
    {
        var sum = 0d;

        for (var i = 0; i < sumNumsAmount; i++)
        {
            sum += Random.Shared.NextDouble();
        }

        return sum;
    }
}