namespace SystemModeling.Lab1.Generators.Interfaces;

internal interface IGenerator
{
    ValueTask<double[]> Generate();
    object GetOptions();
}