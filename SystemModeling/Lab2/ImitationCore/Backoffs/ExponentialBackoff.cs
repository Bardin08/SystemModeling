using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Options.Backoffs;

namespace SystemModeling.Lab2.ImitationCore.Backoffs;

public class ExponentialBackoff : IBackoffStrategy
{
    private readonly TimeSpan _baseDelay;
    private readonly double _lambda;

    public ExponentialBackoff(object options)
    {
        if (options is not ExponentialBackoffOptions expOptions)
        {
            throw new ArgumentException(
                $"Invalid options type. Options must be {nameof(ExponentialBackoffOptions)}");
        }

        if (expOptions.BaseDelay < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(expOptions.BaseDelay),
                expOptions.BaseDelay,
                "should be >= 0ms");
        }

        if (expOptions.Lambda <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(expOptions.Lambda),
                expOptions.Lambda,
                "should be > 0");
        }

        _baseDelay = expOptions.BaseDelay;
        _lambda = expOptions.Lambda;
    }

    public TimeSpan GetBackoff()
    {
        var r = Random.Shared.NextDouble();
        var generatedNumber = -1 * (Math.Log(r) / _lambda);

        return _baseDelay * generatedNumber;
    }
}