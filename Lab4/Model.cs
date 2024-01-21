namespace Lab4;

internal class Model(List<Element> elements)
{
    private double _nextTime;
    private double _currentTime;
    private int _event;

    public void Simulate(double time)
    {
        while (_currentTime < time)
        {
            _nextTime = double.MaxValue;

            elements.Where(e => e.NextTime < _nextTime)
                .ToList()
                .ForEach(e =>
                {
                    _nextTime = e.NextTime;
                    _event = e.Id;
                });

            Console.WriteLine($"\nIt's time for event in {elements[_event].Name}" +
                              $"\nTime = {_nextTime}");

            foreach (var element in elements)
            {
                element.DoStatistics(_nextTime - _currentTime);
            }

            _currentTime = _nextTime;

            elements.ForEach(e => { e.CurrentTime = _currentTime; });

            elements[_event].OutAct();

            elements.ForEach(e =>
            {
                if (Math.Abs(e.NextTime - _currentTime) < 0.0001)
                {
                    e.OutAct();
                }
            });

            PrintInfo();
        }

        PrintResult();
    }

    private void PrintInfo()
    {
        elements.ForEach(e => e.PrintInfo());
    }

    private void PrintResult()
    {
        Console.WriteLine("\n\t-== RESULTS ==-");
        elements.ForEach(e =>
        {
            e.PrintResult();

            if (e is Process process)
            {
                Console.WriteLine(
                    $"\nMean queue length: {process.MeanQueueLength / _currentTime}" +
                    $"\nFailure chance: {process.Failures / (double)process.Quantity} Q {process.CurrentQueueSize}" +
                    $"\nCurrent Time: {_currentTime}");
            }
        });
    }
}