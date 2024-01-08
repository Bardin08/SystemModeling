namespace Lab3.BankSimulation;

internal class Simulation
{
    private readonly Cashier _cashier1;
    private readonly Cashier _cashier2;

    private double _currentTime;
    private double SimulationEndTime { get; init; }
    private int MaxQueueSize { get; init; }


    public Simulation(double simulationEndTime, int maxQueueSize)
    {
        _cashier1 = new Cashier(maxQueueSize);
        _cashier2 = new Cashier(maxQueueSize);

        SimulationEndTime = simulationEndTime;
        MaxQueueSize = maxQueueSize;

        InitializeQueues();
        RunSimulation();
    }

    private void InitializeQueues()
    {
        const int clientsPerQueue = 2;
        for (var i = 0; i < clientsPerQueue; i++)
        {
            _cashier1.TryAddClient(new Client(0, GenerateArrivalTime()), _currentTime);
            _cashier2.TryAddClient(new Client(0, GenerateArrivalTime()), _currentTime);
        }
    }

    private void AddClient(Cashier cashier)
    {
        var client = new Client(_currentTime, GenerateArrivalTime());
        cashier.TryAddClient(client, _currentTime);
    }

    private void RunSimulation()
    {
        while (_currentTime < SimulationEndTime)
        {
            _currentTime += GenerateArrivalTime();

            ProcessNewClient();
            UpdateQueuesAndService();
            _cashier1.GetStats();
        }
    }

    private double GenerateArrivalTime()
    {
        // reversed proportion to mathematical expectation (ME).
        // It ME is 0.5 lambda is 2                        
        const double lambda = 0.5;
        return -Math.Log(1 - Random.Shared.NextDouble()) / lambda;
    }

    private void ProcessNewClient()
    {
        var cashier = GetQueueForNewClient();
        if (cashier is null)
        {
            // TODO: stats (client went out)
            return;
        }

        AddClient(cashier);
    }

    private Cashier? GetQueueForNewClient()
    {
        var q1Size = _cashier1.QueueSize;
        var q2Size = _cashier2.QueueSize;

        // Check if both queues are full
        if (q1Size == MaxQueueSize)
        {
            return q2Size == MaxQueueSize ? null : _cashier2;
        }

        // len(q1) == len(q2) -> q1
        // min(len(q1, q2)) -> q(1|2)
        return q1Size.CompareTo(q2Size) switch
        {
            > 0 => _cashier2,
            < 0 => _cashier1,
            _ => _cashier1
        };
    }

    private void UpdateQueuesAndService()
    {
        _cashier1.DoTick(_currentTime);
        _cashier2.DoTick(_currentTime);

        CheckForQueueSwitch();
    }

    private void CheckForQueueSwitch()
    {
        _cashier1.BalanceQueues(_cashier2, _currentTime);
        _cashier2.BalanceQueues(_cashier1, _currentTime);
    }
}