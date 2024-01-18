namespace Lab3.Hospital;

internal class Hospital
{
    private readonly HospitalQueue _receptionQueue = new();
    private readonly HospitalQueue _escortQueue = new();

    private readonly List<Doctor> _doctors = [new Doctor(), new Doctor()];
    private readonly List<Guide> _guides = [new Guide(), new Guide(), new Guide()];

    private readonly List<Patient> _inLab = [];
    private readonly List<Patient> _completedPatients = [];

    public double GlobalTime { get; private set; }

    public void GenerateNewPatients()
    {
        var patient = Patient.Create(GlobalTime);
        _receptionQueue.Enqueue(patient);
        // Console.WriteLine($"New patient of type {patient.Type} generated and queued.");
    }

    public void PrintStatistics()
    {
        Console.WriteLine("Checked: {0},\nNot Checked: {1}",
            _completedPatients.Count(x => x.Type is PatientType.PreChecked or PatientType.PartiallyChecked),
            _completedPatients.Count(x => x.Type == PatientType.NotChecked));

        for (var index = 0; index < _completedPatients.Count; index++)
        {
            var patient = _completedPatients[index];
            Console.WriteLine("{0} | Time in hospital: {1}. Lab arrival: {2}",
                index,
                patient.Time - patient.CreationTime,
                patient.Time - patient.LabArrival);
        }
    }

    public void ProcessPatients()
    {
        DoTick();

        // at this point we send some patients to lab and some was assigned to guides

        // also here we have a bunch of ones who returned from lab
        // and we have to add them to registration queue again

        var patientsWithFinishedLab = _inLab
            .Where(x => GlobalTime >= x.Time)
            .ToList();
        _inLab.RemoveAll(p => patientsWithFinishedLab.Contains(p));

        var requeuePatients = patientsWithFinishedLab
            .Where(x => x.Type is PatientType.PartiallyChecked)
            .ToList();
        foreach (var patient in requeuePatients)
        {
            patient.Type = PatientType.PreChecked;
            _receptionQueue.Enqueue(patient);
        }

        // Patients of the 3rd time finish here.
        // Add them to collection to get their processing time later
        _completedPatients.AddRange(patientsWithFinishedLab.Except(requeuePatients));
    }

    private void DoTick()
    {
        var doctorsTimeDelta = ProcessDoctors();
        var guidesTimeDelta = ProcessGuides();

        GlobalTime += Math.Max(doctorsTimeDelta, guidesTimeDelta);
    }

    private double ProcessDoctors()
    {
        var timeDelta = 0d;
        foreach (var doc in _doctors)
        {
            if (_receptionQueue.Count == 0)
            {
                continue;
            }

            if (doc.TryAssignPatient(_receptionQueue.Peak(), GlobalTime))
            {
                var patient = _receptionQueue.Dequeue();

                switch (patient.Type)
                {
                    case PatientType.PreChecked:
                        _escortQueue.Enqueue(patient);
                        break;
                    case PatientType.PartiallyChecked or
                        PatientType.NotChecked:
                        patient.DoAnalysis();
                        _inLab.Add(patient);
                        break;
                }
            }

            timeDelta = _doctors.Max(x => x.Time);
        }

        return timeDelta;
    }

    private double ProcessGuides()
    {
        var timeDelta = 0d;
        foreach (var guide in _guides)
        {
            if (_escortQueue.Count == 0)
            {
                continue;
            }

            if (guide.TryAssignPatient(_escortQueue.Peak(), GlobalTime))
            {
                var patient = _escortQueue.Dequeue();
                _completedPatients.Add(patient);
            }

            timeDelta = _guides.Max(x => x.Time);
        }

        return timeDelta;
    }
}