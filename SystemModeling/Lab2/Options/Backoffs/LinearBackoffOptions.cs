using SystemModeling.Lab2.ImitationCore.Interfaces;

namespace SystemModeling.Lab2.Options.Backoffs;

internal record LinearBackoffOptions(
    TimeSpan MinDelay,
    TimeSpan MaxDelay) : IBackoffOptions;