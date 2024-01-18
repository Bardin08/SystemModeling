namespace Lab3.Hospital;

internal partial class Patient
{
    public PatientType Type { get; set; }
    public double Time { get; private set; }
    public double CreationTime { get; private init; }
    public double LabArrival { get; private set; }

    public void DoEscort()
    {
        Time += TimeGenerator.GenerateUniformTime(3, 8);
    }

    public void DoAnalysis()
    {
        var toLabTime = TimeGenerator.GenerateUniformTime(2, 5);
        LabArrival = Time + toLabTime;

        var labReception = TimeGenerator.GenerateErlangTime(4.5, 3);
        var analysisTime = TimeGenerator.GenerateErlangTime(4, 2);
        var fromLabTime = TimeGenerator.GenerateUniformTime(2, 5);

        Time += toLabTime +
                labReception +
                analysisTime +
                fromLabTime;
    }
}

internal partial class Patient
{
    /// <summary>
    /// Create a new patient according to rules:
    /// <list type="table">
    ///   <listheader>
    ///     <term>Patient Type</term>
    ///     <description>Distribution</description>
    ///     <description>Average Time</description>
    ///   </listheader>
    ///   <item>
    ///     <term>PreChecked(1)</term>
    ///     <description>0.5</description>
    ///     <description>15</description>
    ///   </item>
    ///   <item>
    ///     <term>PartiallyChecked (2)</term>
    ///     <description>0.1</description>
    ///     <description>40</description>
    ///   </item>
    ///   <item>
    ///     <term>NotChecked (3)</term>
    ///     <description>0.4</description>
    ///     <description>30</description>
    ///   </item>
    /// </list>
    /// </summary>
    public static Patient Create(double globalTime)
    {
        var patientType = GetPatientType();
        var arrivalTime = TimeGenerator.GenerateExponentialTime(15);
        var avgProcessingTime = patientType switch
        {
            PatientType.PreChecked => 15,
            PatientType.PartiallyChecked => 40,
            PatientType.NotChecked => 30,
            _ => throw new ArgumentOutOfRangeException()
        };

        var creationTime = globalTime + arrivalTime;

        var patient = new Patient
        {
            Type = patientType,
            Time = creationTime + avgProcessingTime,
            CreationTime = creationTime
        };
        return patient;

        PatientType GetPatientType()
        {
            var cumulated = Random.Shared.NextDouble();

            const double preCheckedChance = 0.5;
            const double partiallyChecked = 0.1;
            const double notChecked = 0.4;

            return cumulated switch
            {
                <= preCheckedChance => PatientType.PreChecked,
                <= preCheckedChance + partiallyChecked => PatientType.PartiallyChecked,
                <= preCheckedChance + partiallyChecked + notChecked => PatientType.NotChecked,
                _ => PatientType.NotChecked
            };
        }
    }
}