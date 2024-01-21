namespace Lab4;

public abstract class Element
{
    public string Name { get; set; }
    public double NextTime { get; set; }
    public double DelayMean { get; set; }
    public double DelayDev { get; set; }
    public string Distribution { get; set; }
    public int Quantity { get; private set; }
    public double CurrentTime { get; set; }
    public int State { get; set; }
    public static int NextId { get; set; } = 0;
    public int Id { get; set; }

    public Element? NextElement { get; set; }

    public Element(double delay)
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

    public virtual void InAct()
    {
    }

    public virtual void OutAct()
    {
        Quantity++;
    }

    public double GetDelay()
    {
        return Distribution switch
        {
            "exp" => DelayGenerator.Exp(DelayMean),
            "norm" => DelayGenerator.Norm(DelayMean, DelayDev),
            "unif" => DelayGenerator.Unif(DelayMean, DelayDev),
            _ => DelayMean
        };
    }

    public virtual void PrintResult()
    {
        Console.WriteLine(
            $"{Name}: quantity {Quantity}. Loading {DelayMean * Quantity / CurrentTime}");
    }

    public virtual void PrintInfo()
    {
        Console.WriteLine(
            $"{Name}: state = {State}, quantity = {Quantity}, next = {NextTime}");
    }

    public virtual void DoStatistics(double delta)
    {
    }
}