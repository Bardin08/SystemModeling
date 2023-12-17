using SystemModeling.Trumpee.Configuration;

namespace SystemModeling.Trumpee;

public class FactorExperiment
{
    [JsonProperty("name")] public string? Name { get; set; }
    public int TimesToRun { get; set; }
    [JsonProperty("config")] public TrumpeeSimulationOptions? Configurations { get; set; }
}