namespace SystemModeling.Lab2.Fluent.Interfaces;

internal interface IProcessorNodeBuilder
{
    /// <summary>
    /// Sets the maximum length for the processor queue.
    /// </summary>
    /// <param name="maxLength">The maximum length of the queue. Use -1 for an unbounded queue.</param>
    /// <returns>Returns an instance of IProcessorNodeBuilder for method chaining.</returns>
    IProcessorNodeBuilder SetMaxLength(int maxLength = -1);
    IProcessorNodeBuilder AddTransition(string nextProcessor, double chance);
}