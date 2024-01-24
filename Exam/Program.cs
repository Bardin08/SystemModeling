using Exam;

var proc2 = new Process(3.0, new Process.Queue())
{
    DelayDev = 1,
    Name = "processInfo2"
};

var proc1 = new Process(3.0, new Process.Queue())
{
    DelayDev = 1,
    Name = "processInfo1",
    Distribution = "norm"
};

var dataCollector = new Process(1.0, new Process.Queue())
{
    NextElement = proc2,
    Name = "Receive Data",
    Distribution = "norm"
};

var computer = new Process(2.0, new Process.Queue())
{
    Name = "computer1",
    Distribution = "norm",
    Transitions =
    {
        { proc1, 0.75 },
        { proc2, 0.25 }
    }
};

var create = new Create(2.0)
{
    DelayDev = 1,
    Distribution = "norm",
    Name = "CREATOR",
    NextElement = computer
};

List<Element> list =
[
    create,
    computer,
    proc1,
    dataCollector,
    proc2
];

var model = new Model(list);
model.Simulate(1000.0);