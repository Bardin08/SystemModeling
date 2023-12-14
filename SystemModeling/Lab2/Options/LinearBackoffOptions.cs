using SystemModeling.Lab2.ImitationCore.Interfaces;

namespace SystemModeling.Lab2.ImitationCore.Backoffs;

internal class LinearBackoffOptions : IBackoffOptions
{
    public TimeSpan MinDelay { get; set; }
    public TimeSpan MaxDelay { get; set; }
}