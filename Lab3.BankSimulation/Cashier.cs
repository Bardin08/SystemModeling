using System.Text;

namespace Lab3.BankSimulation;

internal class Cashier(int maxQueueSize)
{
    private readonly CashierStats _stats = new();
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

        _stats.TotalQueueSwaps++;
        var lastClient = cashier.DequeueLastClient();
        TryAddClient(lastClient, currentTime);
    }

    private Client DequeueLastClient()
    {
        return _queue.DequeueLast();
    }

    public void DoTick(double currentTime)
    {
        _stats.Ticks++;

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

    public void PrintStats()
    {
        var sb = new StringBuilder();
        sb.Append("\t -==- Cashier stats -==-")
            .AppendLine()
            .Append($"Total Serving Time: {_stats.TotalServingTime}").AppendLine()
            .Append($"Total Waiting Time: {_stats.TotalWaitingTime}").AppendLine()
            .Append($"Total Served Clients: {_stats.TotalServed}").AppendLine()
            .Append($"Total Reject Clients: {_stats.TotalRejected}").AppendLine()
            .Append($"Total Queue Swaps: {_stats.TotalQueueSwaps}").AppendLine()
            .Append($" \t\t -===- ").AppendLine()
            .Append($"Average Queue Length: {_stats.AverageQueueLength}").AppendLine()
            .Append($"Average Customer Stay Time: {_stats.AverageCustomerStayTime}").AppendLine()
            .Append($"Average Cashier Load: {_stats.AverageLoadPerCashier}").AppendLine()
            .Append($"Percentage of Rejected Clients: {_stats.PercentageOfRejectedCustomers}").AppendLine();
        Console.WriteLine(sb.ToString());
    }
    private void CollectStats(Client client, double currentTime, bool isServed)
    {
        _stats.TotalQueueLength += QueueSize;

        if (isServed)
        {
            _stats.TotalServed++;
            _stats.TotalServingTime += client.ServiceDuration;
            _stats.TotalWaitingTime = currentTime - _stats.TotalServingTime;
        }
        else
        {
            _stats.TotalRejected++;
        }
    }
}