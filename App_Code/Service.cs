using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ConfigurationDataLibrary;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
[DataContract]
public class ALG3Service : IService
{
    [DataMember]
    public ConfigurationData configurationData;


    public List<string> GetAllocations(ConfigurationData configData)
    {
        List<string> allocationList = new List<string>();
        configurationData = configData;
        double[,] taskProTimes = GetTaskProcessorTimes();
        allocationList = HeuristicAlgorithm(taskProTimes);

        //Get Server address
        string serverAddress = System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
        allocationList.Insert(0, serverAddress);

        return allocationList;
    }

    //This algorithm adds tasks to processors starting from the longest task and working backwards till no more tasks can be added
    private List<string> HeuristicAlgorithm(double[,] taskProcessorTimes)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int processors = taskProcessorTimes.GetLength(0);
        int tasks = taskProcessorTimes.GetLength(1);

        double[,] allocation = new double[processors, tasks];
        List<double[,]> goodAllocations = new List<double[,]>();

        double time = 0;
        double energy = 0;

        Random rnd = new Random();
        double bestEnergy = double.MaxValue;

        int allocationCount = 0;

        while (stopwatch.ElapsedMilliseconds <= configurationData.AlgorithmMaxRuntime)
        {

            //Generate an allocation
            allocation = new double[processors, tasks];
            List<int> calculatedProcessors = new List<int>();
            List<int> calculatedTasks = new List<int>();
            int selectedProcessor = rnd.Next(0, processors);


            for (int processor = 0; processor < processors; processor++)
            {
                double processorRuntime = 0;

                for (int task = 0; task < tasks; task++)
                {
                    Dictionary<int, double> selectedProcessorTasks = new Dictionary<int, double>();

                    for (int i = 0; i < tasks; i++)
                    {
                        //only adds tasks not yet assigned
                        if (!calculatedTasks.Contains(i + 1))
                        {
                            selectedProcessorTasks.Add(i + 1, taskProcessorTimes[selectedProcessor, i]);
                        }
                    }

                    var orderedDict = selectedProcessorTasks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                    //Adds longest task then adds the next longest if under time limit
                    for (int i = 0; i < orderedDict.Count(); i++)
                    {
                        if (calculatedTasks.Count < tasks)
                        {

                            if (processorRuntime + orderedDict.ElementAt(i).Value < configurationData.ProgramMaxDuration)
                            {
                                allocation[selectedProcessor, orderedDict.ElementAt(i).Key - 1] = orderedDict.ElementAt(i).Value;
                                selectedProcessorTasks.Remove(orderedDict.ElementAt(i).Key);
                                

                                calculatedTasks.Add(orderedDict.ElementAt(i).Key);
                                processorRuntime += orderedDict.ElementAt(i).Value;

                                //Change dic value to 0
                                orderedDict[orderedDict.ElementAt(i).Key] = 0;
                            }
                        }
                    }

                }

                //Get next processor randomly excluding already calculated processors
                calculatedProcessors.Add(selectedProcessor);

                while (calculatedProcessors.Contains(selectedProcessor))
                {
                    if (calculatedProcessors.Count == processors) break;
                    selectedProcessor = rnd.Next(0, processors);
                }
            }

            allocationCount++;
            double[,] binaryMatrix = ConvertToBinaryMatrix(allocation);

            //Check allocation is valid
            if (ValidateAllocation(binaryMatrix))
            {
                //Compare energy of allocation to goodAllocations
                time = CalculateTime(allocation);
                energy = CalculateEnergy(allocation);


                if (energy < bestEnergy && time <= configurationData.ProgramMaxDuration)
                {
                    goodAllocations.Clear();
                    goodAllocations.Add(allocation);
                    bestEnergy = energy;
                }
                else if (energy == bestEnergy && time <= configurationData.ProgramMaxDuration)
                {
                    goodAllocations.Add(allocation);
                }
            }
        }
        stopwatch.Stop();

        List<string> stringMatrixList = new List<string>();

        //Convert best Allocations to string.
        for (int i = 0; i < goodAllocations.Count; i++)
        {
            stringMatrixList.Add(MatrixToString(goodAllocations[i]));
        }

        //Remove duplicates and convert back to list
        ICollection<string> noDuplicates = new HashSet<string>(stringMatrixList);
        List<string> allocations = noDuplicates.ToList();

        return allocations;
    }

    private double CalculateEnergy(double[,] allocation)
    {
        double energy = 0;

        Dictionary<int, double> processorTimes = new Dictionary<int, double>();

        double c0 = configurationData.CoefficientValues[0];
        double c1 = configurationData.CoefficientValues[1];
        double c2 = configurationData.CoefficientValues[2];

        //get processor times
        for (int processor = 0; processor < allocation.GetLength(0); processor++)
        {
            double processorTime = 0;
            for (int task = 0; task < allocation.GetLength(1); task++)
            {
                processorTime += allocation[processor, task];
            }
            processorTimes.Add(processor + 1, processorTime);
        }


        foreach (KeyValuePair<int, double> time in processorTimes)
        {

            double f = configurationData.ProcessorFrequencies[time.Key];
            energy += time.Value * (c2 * (f * f) + c1 * f + c0);

        }

        return energy;
    }

    private double[,] ConvertToBinaryMatrix(double[,] allocation)
    {
        int rows = allocation.GetLength(0);
        int columns = allocation.GetLength(1);
        double[,] oneZeroMatrix = new double[rows, columns];


        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (allocation[i, j] > 0) oneZeroMatrix[i, j] = 1;
                else oneZeroMatrix[i, j] = 0;
            }

        }

        return oneZeroMatrix;
    }

    private double CalculateTime(double[,] allocation)
    {
        double time = 0;

        for (int processor = 0; processor < allocation.GetLength(0); processor++)
        {
            double rowSum = 0;
            for (int task = 0; task < allocation.GetLength(1); task++)
            {
                rowSum += allocation[processor, task];
            }
            if (rowSum > time)
            {
                time = rowSum;
            }
        }

        return time;
    }

    private double[,] GetTaskProcessorTimes()
    {
        int rows = configurationData.ProgramProcessors;
        int columns = configurationData.ProgramTasks;
        double[,] taskProcessorTimes = new double[rows, columns];
        if (configurationData.TaskRuntimes != null)
        {
            for (int processors = 0; processors < rows; processors++)
            {
                for (int tasks = 0; tasks < columns; tasks++)
                {
                    double time = configurationData.TaskRuntimes[tasks + 1] * (configurationData.RuntimeReferenceFrequency / configurationData.ProcessorFrequencies[processors + 1]);
                    taskProcessorTimes[processors, tasks] = time;
                }
            }
        }

        return taskProcessorTimes;
    }

    private string MatrixToString(double[,] matrix)
    {

        //Change numbers to 1 or 0
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i, j] > 0) matrix[i, j] = 1;
                else matrix[i, j] = 0;
            }
        }

        string str = "";
        int rows = matrix.GetLength(0);
        int columns = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                str += matrix[i, j] + ",";
            }

            //Remove last ,
            str = str.Substring(0, str.Length - 1);
            str += "\n";
        }
        //remove last \n
        str = str.Substring(0, str.Length - 1);

        return str;
    }

    public bool ValidateAllocation(double[,] allocation)
    {
        bool isValid = true;
        int rows = allocation.GetLength(0);
        int columns = allocation.GetLength(1);

        for (int tasks = 0; tasks < columns; tasks++)
        {
            double columnSum = 0;
            for (int processors = 0; processors < rows; processors++)
            {
                //Add each column to determine if there is more or less than one 1 
                columnSum += allocation[processors, tasks];
            }


            if (columnSum != 1)
            {
                //errors.Add(invalidAllocationError + ToString());
                isValid = false;
            }

        }

        return isValid;
    }
}