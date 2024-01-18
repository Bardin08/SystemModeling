namespace Lab3.Hospital;

internal static class TimeGenerator
{
    public static double GenerateExponentialTime(double mean)
    {
        return -Math.Log(1 - Random.Shared.NextDouble()) * mean;
    }

    public static double GenerateUniformTime(double min, double max)
    {
        var tmp = Random.Shared.NextDouble();
        tmp = min * tmp % max;
        var generatedNumber = tmp / max;
        return generatedNumber;
    }

    public static double GenerateErlangTime(double mean, int k)
    {
        double sum = 0;
        for (var i = 0; i < k; i++)
        {
            sum += GenerateExponentialTime(mean / k);
        }

        return sum;
    }
}