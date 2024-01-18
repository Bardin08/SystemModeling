namespace Lab3.Hospital;

internal class Doctor
{
    public double Time { get; private set; }

    public bool TryAssignPatient(Patient patient, double globalTime)
    {
        if (Time > globalTime)
        {
            return false;
        }

        // add delta for current patient
        Time += patient.Time - Time;
        return true;
    }
}