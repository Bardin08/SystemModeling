namespace Lab4;

internal class Process(double delay, Process.Queue queue) : Element(delay)
{
    public int CurrentQueueSize => queue.Value;
    public int Failures { get; private set; }
    public double MeanQueueLength { get; private set; }
    public int QueueLengthThreshold { get; init; } = int.MaxValue;

    public Dictionary<Process, double> Transitions { get; } = new();

    public override void InAct()
    {
        if (State == 0)
        {
            State = 1;
            CurrentTime += GetDelay();
        }
        else
        {
            if (queue.Value < QueueLengthThreshold)
            {
                queue.Enqueue();
            }
            else
            {
                TryProcessWithNextProcess();
            }
        }
    }

    public override void OutAct()
    {
        base.OutAct();

        NextTime = double.MaxValue;
        State = 0;

        if (queue.Value <= 0)
        {
            return;
        }

        queue.Dequeue();
        State = 1;
        NextTime = CurrentTime + GetDelay();
    }

    private void TryProcessWithNextProcess()
    {
        if (Transitions.Count is 0)
        {
            TryProcessWithNextElement();
            return;
        }

        var randomProbability = Random.Shared.NextDouble();

        var cumulative = 0d;
        foreach (var (processor, chance) in Transitions)
        {
            cumulative += chance;

            var processorFound = cumulative >= randomProbability;
            if (!processorFound)
            {
                continue;
            }

            processor.InAct();
            break;
        }
    }

    private void TryProcessWithNextElement()
    {
        var nextElement = NextElement;
        if (nextElement is null)
        {
            if (queue.Value < QueueLengthThreshold)
            {
                queue.Enqueue();
            }
            else
            {
                Failures++;
            }
        }
        else
        {
            nextElement.InAct();
        }
    }

    public override void PrintInfo()
    {
        base.PrintInfo();
        Console.WriteLine($"failure = {Failures}");
    }

    public override void DoStatistics(double delta)
    {
        MeanQueueLength = QueueLengthThreshold + queue.Value * delta;
    }

    internal class Queue
    {
        public int Value { get; private set; }

        public void Enqueue()
        {
            Value++;
        }

        public void Dequeue()
        {
            Value--;
        }
    }
}