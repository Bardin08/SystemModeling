using SystemModeling.Common;

var executionArgs = new Dictionary<string, object>()
{
    // { "context", "Lab2: Object oriented mass-serving system imitation." }
    { "context", "Trumpee - distributed notifications system imitation" }
};

var runner = new FancyFeaturesRunnable();
await runner.RunAsync(executionArgs);