using SystemModeling.Common;

var executionArgs = new Dictionary<string, object>()
{
    { "context", "Lab2: Object oriented mass-serving system imitation." }
};

var runnable = new Lab2Runnable();
await runnable.RunAsync(executionArgs);