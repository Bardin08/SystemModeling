namespace SystemModeling.Lab1.Interfaces;

internal interface IGenerator
{
    ValueTask<double[]> Generate();
}