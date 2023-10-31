namespace SystemModeling.Lab1.Analytics.Collectors;

internal record FrequencyMapBucket : IComparable<FrequencyMapBucket>
{
    public double Min { get; init; }
    public double Max { get; init; }
    public int ItemsAmount { get; init; }

    public override int GetHashCode()
    {
        return HashCode.Combine(Min, Max, ItemsAmount);
    }

    public override string ToString()
    {
        return $"{Min,6:0.00} - {Max,6:0.00} : {ItemsAmount,6}";
    }

    public virtual bool Equals(FrequencyMapBucket? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        const double tolerance = 0.0000001;
        return Math.Abs(Min - other.Min) < tolerance &&
               Math.Abs(Max - other.Max) < tolerance &&
               ItemsAmount == other.ItemsAmount;
    }

    public int CompareTo(FrequencyMapBucket? other)
    {
        if (other is null)
        {
            return 1;
        }

        var thisRange = Max + Min;
        var otherRange = other.Max + other.Min;

        return thisRange.CompareTo(otherRange);
    }
}