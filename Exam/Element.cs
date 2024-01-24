namespace Exam;

public abstract class Element
{
    private double DelayMean { get; set; }

    public string Name { get; init; }
    public string Distribution { get; init; }
    public Element? NextElement { get; init; }

    public int Id { get; set; }
    public double DelayDev { get; set; }
    public double CurrentTime { get; set; }

    protected int State { get; set; }
    private static int NextId { get; set; }

    public int Quantity { get; private set; }
    public double NextTime { get; protected set; }

    protected Element(double delay)
    {
        NextTime = 0.0;
        DelayMean = delay;
        Distribution = "";
        CurrentTime = NextTime;
        State = 0;
        NextElement = null;
        Id = NextId;
        NextId++;
        Name = "element" + Id;
    }

    protected double GetDelay()
    {
        return Distribution switch
        {
            "exp" => DelayGenerator.Exp(DelayMean),
            "norm" => DelayGenerator.Norm(DelayMean, DelayDev),
            "unif" => DelayGenerator.Unif(DelayMean, DelayDev),
            _ => DelayMean
        };
    }

    public virtual void PrintInfo()
    {
        Console.WriteLine(
            $"{Name}: state = {State}, quantity = {Quantity}, next = {NextTime}");
    }

    public virtual void PrintResult()
    {
        Console.WriteLine(
            $"{Name}: quantity {Quantity}. Loading {DelayMean * Quantity / CurrentTime}");
    }

    public virtual void OutAct()
    {
        Quantity++;
    }

    public virtual void InAct()
    {
    }

    public virtual void DoStatistics(double delta)
    {
    }
}