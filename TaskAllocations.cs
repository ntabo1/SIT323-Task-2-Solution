using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIT323AssessmentTask2
{
    public class TaskAllocations
    {
        #region Constants
        private const char delimiter = ',';
        private const string comment = "//";
        private const string doubleQuote = "\"";
        private const string emptySpace = "";

        //Errors
        private const string missingKeyError = "TAN File is missing a key: ";
        private const string multipleKeyError = "TAN File contains multiple keys: ";
        private const string invalidLineError = "Invalid line in TAN file: ";
        private const string commentLineError = "Invalid comment on line: ";
        private const string stringToIntError = "String to Int error: ";
        private const string missingSeperatorError = "Invalid seperator: ";
        private const string invalidSeperatorError = "Invalid seperator on line: ";
        private const string diffNoAllocationToExpectedError = "Different number of Allocations than expected. {0} of {1} expected";
        private const string invalidFileError = "File type is invalid, must be .tan: ";
        private const string invalidAllocationError = "Invalid Allocation: ";

        //Regex
        private const string configFilePattern = @"CONFIGURATION";
        private const string tasksPattern = @"TASKS";
        private const string processorsPattern = @"PROCESSORS";
        private const string allocationsPattern = @"ALLOCATIONS";
        private const string allocationDigitPattern = @"ALLOCATIONS,\d+";
        private const string allocationIDPattern = @"ALLOCATION-ID";
        private const string tanFilePattern = @".+\.tan$";
        private const string seperatorPattern = @".+,.+";
        #endregion

        #region Properties
        //Keys
        private readonly string configPathKey = "CONFIGURATION";
        private readonly string tasksKey = "TASKS";
        private readonly string processorsKey = "PROCESSORS";
        private readonly string allocationsKey = "ALLOCATIONS";
        private readonly string allocationIDKey = "ALLOCATION-ID";

        //Values
        public string ConfigPath { get; set; }
        public int Tasks { get; set; }
        public int Processors { get; set; }
        public int NumAllocations { get; set; }
        public List<Allocation> SetOfAllocations { get; set; }

        //Errors
        public bool isValid { get; set; }
        public List<String> AllocationErrorList= new List<string>();

        //Filename
        public string taskAllocationPath { get; set; }
        #endregion

        #region Constructers
        public TaskAllocations(string filePath)
        {
            taskAllocationPath = filePath;
        }

        public TaskAllocations()
        {

        }
        #endregion

        #region Methods
        //Helper method to convert strings to Int32
        public static int ToInt32(string input)
        {
            if (Int32.TryParse(input, out int anInt))
            {
                return anInt;
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// Validates that all keys are present, checks that the number of allocations is equal to the number expected, validates the allocations and the filename.
        /// </summary>
        /// <returns>Bool determined by any errors present</returns>
        public bool Validate()
        {

            StreamReader file = new StreamReader(taskAllocationPath);
            string tanContents = file.ReadToEnd();
            file.Close();

            //Check file contains all keys
            Regex configPathRegex = new Regex(configFilePattern);
            MatchCollection configPathMatch = configPathRegex.Matches(tanContents);
            if (configPathMatch.Count == 0)
            {
                AllocationErrorList.Add(missingKeyError + configPathKey);
            }
            if(configPathMatch.Count > 1)
            {
                AllocationErrorList.Add(multipleKeyError + configPathKey);
            }

            Regex tasksRegex = new Regex(tasksPattern);
            MatchCollection tasksMatch = tasksRegex.Matches(tanContents);
            if (tasksMatch.Count == 0)
            {
                AllocationErrorList.Add(missingKeyError + tasksKey);
            }
            if (tasksMatch.Count > 1)
            {
                AllocationErrorList.Add(multipleKeyError + tasksKey);
            }

            Regex processorsRegex = new Regex(processorsPattern);
            MatchCollection processorsMatch = processorsRegex.Matches(tanContents);
            if (processorsMatch.Count == 0)
            {
                AllocationErrorList.Add(missingKeyError + processorsKey);
            }
            if (processorsMatch.Count > 1)
            {
                AllocationErrorList.Add(multipleKeyError + processorsKey);
            }

            Regex allocationRegex = new Regex(allocationsPattern);
            MatchCollection allocationMatch = allocationRegex.Matches(tanContents);
            if (allocationMatch.Count == 0)
            {
                AllocationErrorList.Add(missingKeyError + allocationsKey);
            }
            if (allocationMatch.Count > 1)
            {
                AllocationErrorList.Add(multipleKeyError + allocationsKey);
            }

            //Check No. of Allocations is correct
            Regex allocationWithDigitRegex = new Regex(allocationDigitPattern);
            MatchCollection allocationWithDigitMatch = allocationWithDigitRegex.Matches(tanContents);
            string line = "";
            int expectedAllocations = -1;

            if (allocationWithDigitMatch.Count > 0)
            {
                line = allocationWithDigitMatch[0].Value;
                string[] substrings = line.Split(delimiter);
                expectedAllocations = ToInt32(substrings[1]);
                if (expectedAllocations == -1) AllocationErrorList.Add(stringToIntError + line);

                Regex allocationIDRegex = new Regex(allocationIDPattern);
                MatchCollection allocationIDMatch = allocationIDRegex.Matches(tanContents);
                int actualAllocations = allocationIDMatch.Count;


                if (expectedAllocations != actualAllocations)
                {
                    AllocationErrorList.Add(string.Format(diffNoAllocationToExpectedError, actualAllocations, expectedAllocations));
                }
            }

            //Validate allocations
            List<string> allocationErrors = new List<string>();
            foreach (Allocation a in SetOfAllocations)
            {
                a.ValidateAllocation(out allocationErrors);
                if (!a.isValid) AllocationErrorList.Add(invalidAllocationError + a.ID);
                AllocationErrorList.AddRange(allocationErrors);
            }
            

            //Check filename is valid
            Regex tanFileRegex = new Regex(tanFilePattern);
            MatchCollection tanFileMatch = tanFileRegex.Matches(taskAllocationPath);
            if (tanFileMatch.Count == 0) AllocationErrorList.Add(invalidFileError + taskAllocationPath);

            isValid = AllocationErrorList.Count == 0;
            return (isValid);
        }
        
        /// <summary>
        /// Determines if a .tan file is valid and parses the data to an instance of TaskAllocation
        /// </summary>
        /// <param name="path">A string containg the filepath of a .tan file</param>
        /// <param name="anAllocation">An instance of TaskAllocation that is populated with data from the .tan file being parsed</param>
        /// <returns>A bool depending on if the file is valid and error free.</returns>
        public static bool TryParse(string path, out TaskAllocations anAllocation)
        {
            
            anAllocation = new TaskAllocations(path);
            List<String> ParsingErrorList = new List<string>();
            anAllocation.SetOfAllocations = new List<Allocation>();

            Regex seperatorRegex = new Regex(seperatorPattern);

            //Begin parsing TAN file
            StreamReader file = new StreamReader(path);
            while (!file.EndOfStream)
            {
                string line = file.ReadLine();
                MatchCollection seperatorMatch = seperatorRegex.Matches(line);

                if(line.Length == 0) { }
                else if (line.Contains(comment))
                {
                    if (line.StartsWith(comment))
                    {
                        //Comment Found
                    }
                    else
                    {
                        ParsingErrorList.Add(commentLineError + line);
                    }
                }
                else if (seperatorMatch.Count == 0)
                {
                    ParsingErrorList.Add(invalidSeperatorError + line);
                }
                else if (line.Contains(anAllocation.configPathKey))
                {
                    string[] substrings = line.Split(delimiter);

                    //Check Line is valid and only contains expected number of arguements
                    if(substrings[0] == anAllocation.configPathKey && substrings.Count() == 2)
                    {
                        string configPath = substrings[1].Replace(doubleQuote, emptySpace);
                        anAllocation.ConfigPath = configPath;
                    }
                    else
                    {
                        ParsingErrorList.Add(invalidLineError + line);
                    }
                }
                else if (line.Contains(anAllocation.tasksKey))
                {
                    string[] substrings = line.Split(delimiter);
                    int input = ToInt32(substrings[1]);

                    if(input == -1) ParsingErrorList.Add(stringToIntError + line);
                    else
                    {
                        anAllocation.Tasks = input;
                    }

                }
                else if (line.Contains(anAllocation.processorsKey))
                {
                    string[] substrings = line.Split(delimiter);

                    int input = ToInt32(substrings[1]);
                    if (input == -1)
                    {
                        string error = stringToIntError + line;
                        ParsingErrorList.Add(error);
                    }
                    else
                    {
                        anAllocation.Processors = input;
                    }
                }
                else if (line.Contains(anAllocation.allocationsKey))
                {
                    string[] substrings = line.Split(delimiter);
                    int input = ToInt32(substrings[1]);
                    if (input == -1)
                    {
                        string error = stringToIntError + line;
                        ParsingErrorList.Add(error);
                    }
                    else
                    {
                        anAllocation.NumAllocations = input;
                    }
                }
                else if (line.Contains(anAllocation.allocationIDKey))
                {
                    string[] substrings = line.Split(delimiter);
                    int id = ToInt32(substrings[1]);
                    if (id == -1)
                    {
                        string error = stringToIntError + line;
                        ParsingErrorList.Add(error);
                    }
                    
                    List<String> allocationMatrix = new List<string>();

                    while (line.Length != 0)
                    {
                        line = file.ReadLine();

                        if (line != emptySpace && line != null)
                        {
                            allocationMatrix.Add(line);
                        }
                        else break;
                    }

                    Allocation aAllocation = new Allocation(id, allocationMatrix);
                    anAllocation.SetOfAllocations.Add(aAllocation);

                }
                else
                {
                    string error = invalidLineError + line;
                    ParsingErrorList.Add(error);
                }
            }
            file.Close();

            //Add parsing errors to instance error list
            anAllocation.AllocationErrorList.AddRange(ParsingErrorList);

            //Check validity of file
            anAllocation.isValid = (anAllocation.Validate() && ParsingErrorList.Count == 0);

            return ParsingErrorList.Count == 0;
        }
        #endregion
    }
}
