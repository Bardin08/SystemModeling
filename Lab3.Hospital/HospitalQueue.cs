namespace Lab3.Hospital;

internal class HospitalQueue
{
    private readonly PriorityQueue<Patient, int> _queue = new();
    public int Count => _queue.Count;

    public void Enqueue(Patient patient)
    {
        var priority = patient.Type switch
        {
            PatientType.PreChecked => 0,
            PatientType.NotChecked or PatientType.PartiallyChecked => 1,
            _ => throw new ArgumentOutOfRangeException()
        };

        _queue.Enqueue(patient, priority);
    }

    public Patient Peak()
    {
        return _queue.Peek();
    }

    public Patient Dequeue()
    {
        return _queue.Dequeue();
    }
}