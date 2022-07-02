using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using SIT323AssessmentTask2;

namespace ConfigurationDataLibrary
{
    [Serializable]
    public class ConfigurationData
    {
        public decimal AlgorithmMaxRuntime { get; set; }
        public string LogFilePath { get; set; }
        
        public int ProgramTasks { get; set; }
        public int ProgramProcessors { get; set; }

        public double ProgramMaxDuration { get; set; }
        public double RuntimeReferenceFrequency { get; set; }

        public Dictionary<int, double> TaskRuntimes { get; set; }
        public Dictionary<int, double> ProcessorFrequencies { get; set; }
        public Dictionary<int, double> CoefficientValues { get; set; }
        
        public ConfigurationData()
        {

        }

        public ConfigurationData(Configuration configuration)
        {
            LogFilePath = configuration.LogFilePath;
            ProgramTasks = configuration.ProgramTasks;
            ProgramProcessors = configuration.ProgramProcessors;
            ProgramMaxDuration = configuration.ProgramMaxDuration;
            RuntimeReferenceFrequency = configuration.RuntimeReferenceFrequency;
            TaskRuntimes = configuration.TaskRuntimes;
            ProcessorFrequencies = configuration.ProcessorFrequencies;
            CoefficientValues = configuration.CoefficientValues;
        }
    }
}
