using SystemModeling.Common;

var executionArgs = new Dictionary<string, object>()
{
    {"context", "Lab1: Number generators. Distribution."}
};

var lab1Runner = new Lab1Runnable();
await lab1Runner.RunAsync(executionArgs);