using System.Text.Json;

namespace Lab3.BankSimulation;

internal class Cashier(int maxQueueSize)
{
    private int _totalServedClients;
    private double _totalServiceTime;
    private int _totalRejectedClients;
    private double _totalWaitTime;

    private readonly Deq<Client> _queue = [];
    public int QueueSize => _queue.Count;

    public bool TryAddClient(Client client, double currentTime)
    {
        if (QueueSize <= maxQueueSize)
        {
            _queue.Enqueue(client);
            return true;
        }

        CollectStats(client, currentTime, false);
        return false;
    }

    public void BalanceQueues(Cashier cashier, double currentTime)
    {
        const int threshold = 2;
        if (cashier.QueueSize <= 1 ||
            QueueSize - cashier.QueueSize < threshold)
        {
            return;
        }

        var lastClient = cashier.DequeueLastClient();
        TryAddClient(lastClient, currentTime);
    }

    private Client DequeueLastClient()
    {
        return _queue.DequeueLast();
    }

    public void DoTick(double currentTime)
    {
        if (QueueSize <= 0)
        {
            return;
        }

        var currentClient = _queue.Peak();

        var servingComplete = currentTime - currentClient.ArrivalTime >= currentClient.ServiceDuration;
        if (!servingComplete)
        {
            return;
        }

        _queue.Dequeue();
        CollectStats(currentClient, currentTime, true);
    }

    private void CollectStats(Client client, double currentTime, bool isServed)
    {
        if (isServed)
        {
            _totalServedClients++;
            _totalServiceTime += client.ServiceDuration;
            _totalWaitTime = currentTime - _totalServiceTime;
        }
        else
        {
            _totalRejectedClients++;
        }
    }

    public void GetStats()
    {
        var stats = new
        {
            _totalServedClients,
            _totalRejectedClients,
            _totalServiceTime,
            _totalWaitTime
        };

        Console.WriteLine(JsonSerializer.Serialize(stats));
    }
}