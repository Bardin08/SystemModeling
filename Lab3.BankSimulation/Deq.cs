namespace Lab3.BankSimulation;

public class Deq<T> : LinkedList<T>
{
    public void Enqueue(T elem)
    {
        ArgumentNullException.ThrowIfNull(elem);
        AddLast(elem);
    }

    public T Peak()
    {
        return First!.Value;
    }

    public T Dequeue()
    {
        if (First is null)
        {
            throw new InvalidOperationException("No elems in deq. Can't dequeue");
        }

        var elem = First!.Value;
        RemoveFirst();
        return elem;
    }

    public T DequeueLast()
    {
        if (Last is null)
        {
            throw new InvalidOperationException("No elems in deq. Can't dequeue");
        }

        var elem = Last!.Value;
        RemoveLast();
        return elem;
    }
}