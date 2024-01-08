namespace Lab3.BankSimulation;

public class CashierStats
{
    public int TotalServed { get; set; }
    public int TotalRejected { get; set; }
    public int TotalQueueSwaps { get; set; }
    public double TotalServingTime { get; set; }
    public double TotalWaitingTime { get; set; }

    public double AverageLoadPerCashier => TotalServingTime / TotalServed;
    public double AverageCustomerStayTime => (TotalServingTime + TotalWaitingTime) / TotalServed;
    public double PercentageOfRejectedCustomers => (double)TotalRejected / TotalServed * 100;

    public double AverageQueueLength => -1;
}