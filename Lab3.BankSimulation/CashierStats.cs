namespace Lab3.BankSimulation;

public record CashierStats(
    int TotalServed,
    int TotalRejected,
    double TotalServingTime,
    double TotalWaitingTime,
    int TotalQueueSwap)
{
    public double AverageLoadPerCashier => TotalServingTime / TotalServed;
    public double AverageCustomerStayTime => (TotalServingTime + TotalWaitingTime) / TotalServed;
    public double PercentageOfRejectedCustomers => (double)TotalRejected / TotalServed * 100;
    public int TotalQueueSwaps => TotalQueueSwap;

    public double AverageQueueLength => -1;
}