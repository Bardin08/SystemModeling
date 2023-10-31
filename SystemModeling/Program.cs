using SystemModeling.Common;

var executionArgs = new Dictionary<string, object>()
{
    { "context", "Lab2: Object oriented mass-serving system imitation." }
};

var lab1Runner = new Lab2Runnable();
await lab1Runner.RunAsync(executionArgs);