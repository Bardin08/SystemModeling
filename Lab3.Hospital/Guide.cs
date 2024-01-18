namespace Lab3.Hospital;

internal class Guide
{
    public double Time { get; private set; }

    public bool TryAssignPatient(Patient patient, double globalTime)
    {
        if (Time > globalTime)
        {
            return false;
        }

        // Console.WriteLine($"Guiding patient with type {patient.Type}..");

        // Add time that was used by guide to move patient to room

        Time += patient.Time - Time;
        patient.DoEscort();

        return true;
    }
}