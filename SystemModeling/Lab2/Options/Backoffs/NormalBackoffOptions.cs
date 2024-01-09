using SystemModeling.Lab2.ImitationCore.Interfaces;

namespace SystemModeling.Lab2.Options.Backoffs;

internal record NormalBackoffOptions(
    TimeSpan MinDelay,
    TimeSpan MaxDelay) : IBackoffOptions;