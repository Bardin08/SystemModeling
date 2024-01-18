using Lab3.Hospital;

const double timeThreshold = 1000;

var hospital = new Hospital();

while (hospital.GlobalTime < timeThreshold)
{
    hospital.GenerateNewPatients();
    hospital.ProcessPatients();
}

hospital.PrintStatistics();

Console.WriteLine("Completed!");