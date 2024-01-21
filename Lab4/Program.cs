using Lab4;

var process3 = new Process(3.0, new Process.Queue())
{
    Name = "PROCESSOR3",
    Distribution = "exp",
    QueueLengthThreshold = 3
};

var process2 = new Process(3.0, new Process.Queue())
{
    Name = "PROCESSOR2",
    Distribution = "exp",
    NextElement = process3,
    QueueLengthThreshold = 3
};

var process1 = new Process(3.0, new Process.Queue())
{
    Name = "PROCESSOR1",
    Distribution = "exp",
    NextElement = process2,
    QueueLengthThreshold = 3,
    Transitions =
    {
        { process2, 0.8 },
        { process3, 0.2 }
    }
};

var create = new Create(2.0)
{
    NextElement = process1,
    Name = "CREATOR",
    Distribution = string.Empty
};


List<Element> list =
[
    create,
    process1,
    process2,
    process3
];

var model = new Model(list);
model.Simulate(500.0);

internal class Create : Element
{
    public Create(double delay) : base(delay)
    {
        NextTime = 0d;
    }

    public override void OutAct()
    {
        base.OutAct();
        NextTime = CurrentTime + GetDelay();
        NextElement?.InAct();
    }
}