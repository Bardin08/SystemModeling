using SystemModeling.Lab2.ImitationCore.Models;

namespace SystemModeling.Lab2.ImitationCore.Interfaces;

internal interface IEventProcessorStateObserver : IObserver
{
    ProcessorStatisticsDto GetProcessorStatistics();
}