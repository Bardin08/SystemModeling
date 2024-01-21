namespace Lab4;

internal class Model
{
    private readonly List<Element> _elements = [];

    private double _nextTime;
    private double _currentTime;
    private int _event;

    public Model(List<Element> elements)
    {
        _elements = elements;
    }

    public void Simulate(double time)
    {
        while (_currentTime < time)
        {
            _nextTime = double.MaxValue;

            foreach (var element in _elements
                         .Where(element => element.NextTime < _nextTime))
            {
                _nextTime = element.NextTime;
                _event = element.Id;
            }

            Console.WriteLine($"\nIt's time for event in {_elements[_event].Name}" +
                              $"\nTime = {_nextTime}");

            foreach (var element in _elements)
            {
                element.DoStatistics(_nextTime - _currentTime);
            }

            _currentTime = _nextTime;

            _elements.ForEach(e => { e.CurrentTime = _currentTime; });

            _elements[_event].OutAct();

            _elements.ForEach(e =>
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
        _elements.ForEach(e => e.PrintInfo());
    }

    private void PrintResult()
    {
        Console.WriteLine("\n\t-== RESULTS ==-");
        _elements.ForEach(e =>
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