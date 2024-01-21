namespace Lab4;

public static class DelayGenerator
{
    /// <summary>
    /// Generates a random value according to an exponential distribution
    /// </summary>
    /// <param name="timeMean">timeMean mean value</param>
    /// <returns>a random value according to an exponential distribution</returns>
    public static double Exp(double timeMean)
    {
        double a = 0;
        while (a == 0)
        {
            a = Random.Shared.NextDouble();
        }

        a = -timeMean * Math.Log(a);
        return a;
    }

    /// <summary>
    /// Generates a random value according to a uniform distribution
    /// </summary>
    /// <param name="timeMin"></param>
    /// <param name="timeMax"></param>
    /// <returns>a random value according to a uniform distribution</returns>
    public static double Unif(double timeMin, double timeMax)
    {
        double a = 0;
        while (a == 0)
        {
            a = Random.Shared.NextDouble();
        }

        a = timeMin + a * (timeMax - timeMin);
        return a;
    }

    /// <summary>
    /// Generates a random value according to a normal (Gauss) distribution
    /// </summary>
    /// <param name="timeMean"></param>
    /// <param name="timeDeviation"></param>
    /// <returns>a random value according to a normal (Gauss) distribution</returns>
    public static double Norm(double timeMean, double
        timeDeviation)
    {
        var r = new Random();
        return timeMean + timeDeviation * r.NextGaussian();
    }
}