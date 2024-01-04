using SystemModeling.Lab2.ImitationCore.Interfaces;
using SystemModeling.Lab2.Options.Backoffs;

namespace SystemModeling.Lab2.ImitationCore.Backoffs;

public class LinearBackoff : IBackoffStrategy
{
    private readonly TimeSpan _minDelay;
    private readonly TimeSpan _maxDelay;

    public LinearBackoff(object options)
    {
        if (options is not LinearBackoffOptions linearOptions)
        {
            throw new ArgumentException($"Invalid options type. Options must be {nameof(LinearBackoffOptions)}");
        }

        if (linearOptions.MinDelay < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(linearOptions.MinDelay),
                linearOptions.MinDelay,
                "should be >= 0ms");
        }

        if (linearOptions.MaxDelay < linearOptions.MinDelay)
        {
            throw new ArgumentOutOfRangeException(
                nameof(linearOptions.MaxDelay),
                linearOptions.MaxDelay,
                "should be >= MinDelay");
        }

        _minDelay = linearOptions.MinDelay;
        _maxDelay = linearOptions.MaxDelay;
    }

    public TimeSpan GetBackoff()
    {
        var delayMs = Random.Shared.Next(
            (int)_minDelay.TotalMilliseconds, (int)_maxDelay.TotalMilliseconds);

        return TimeSpan.FromMilliseconds(delayMs);
    }
}