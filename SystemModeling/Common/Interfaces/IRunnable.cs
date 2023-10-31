namespace SystemModeling.Common.Interfaces;

internal interface IRunnable
{
    Task RunAsync(Dictionary<string, object> args);
}