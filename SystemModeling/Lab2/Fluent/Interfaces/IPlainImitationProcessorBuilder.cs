using SystemModeling.Lab2.Options;

namespace SystemModeling.Lab2.Fluent.Interfaces;

internal interface IPlainImitationProcessorBuilder
{
    IPlainImitationProcessorBuilder AddProcessorOptions(
        Action<ImitationProcessorOptions> optionsBuilder);
}