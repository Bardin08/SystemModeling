using SystemModeling.Lab2.ImitationCore.Interfaces;

namespace SystemModeling.Lab2.Options.Backoffs;

internal record ExponentialBackoffOptions(
    TimeSpan BaseDelay,
    double Lambda) : IBackoffOptions;