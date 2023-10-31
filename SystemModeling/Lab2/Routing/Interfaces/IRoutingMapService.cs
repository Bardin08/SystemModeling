using SystemModeling.Lab2.Routing.Models;

namespace SystemModeling.Lab2.Routing.Interfaces;

public interface IRoutingMapService
{
    ProcessorNode? GetProcessorNodeByName(string processorName);
}