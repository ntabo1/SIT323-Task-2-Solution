using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIT323Assignment1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment1Tests
{
    [TestClass]
    public class TaskAllocationTests
    {
        const string test1FilePath = "C:\\Users\\1530\\source\\repos\\SIT323AssessmentTask2\\Files for Unit Testing\\Test1.tan";

        [TestMethod]
        public void ConfigFileTest()
        {
            //Arrange
            string expectedConfigFile = "Test1.csv";

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            string configFile = test.ConfigPath;

            //Assert
            Assert.AreEqual(expectedConfigFile, configFile, "CONFIGURATION path is incorrect in Test1.tan");

        }

        [TestMethod]
        public void TasksTest()
        {
            //Arrange
            int expectedTasks = 5;

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            int tasks = test.Tasks;

            //Assert
            Assert.AreEqual(expectedTasks, tasks, "TASKS is incorrect in Test1.tan");
        }

        [TestMethod]
        public void ProcessorsTest()
        {
            //Arrange
            int expectedProcessors = 3;

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            int processors = test.Processors;

            //Assert
            Assert.AreEqual(expectedProcessors, processors, "PROCESSORS is incorrect in Test1.tan");

        }

        [TestMethod]
        public void AllocationsTest()
        {
            //Arrange
            int expectedAllocations = 8;

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            int allocations = test.NumAllocations;

            //Assert
            Assert.AreEqual(expectedAllocations, allocations, "ALLOCATIONS is incorrect in Test1.tan");

        }

        [TestMethod]
        public void AllocationID1Test()
        {
            //Arrange
            int expectedID = 1;
            double[,] expectedMatrix1 = { { 1, 1, 0, 0, 0 }, { 0, 0, 1, 1, 0 }, { 0, 0, 0, 0, 1 } };

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            int id = test.SetOfAllocations[0].ID;
            double[,] matrix = test.SetOfAllocations[0].AllocationMatrix;

            //Assert
            Assert.AreEqual(expectedID, id, "ALLOCATION-ID 1 has incorrect ID");
            CollectionAssert.AreEqual(expectedMatrix1, matrix, "ALLOCATION-ID 1 is incorrect in Test1.tan");

        }

        [TestMethod]
        public void AllocationID2Test()
        {
            //Arrange
            int expectedID = 2;
            double[,] expectedMatrix1 = { { 1, 1, 0, 0, 0 }, { 0, 0, 0, 0, 1 }, { 0, 0, 1, 1, 0 } };

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            int id = test.SetOfAllocations[1].ID;
            double[,] matrix = test.SetOfAllocations[1].AllocationMatrix;

            //Assert
            Assert.AreEqual(expectedID, id, "ALLOCATION-ID 2 has incorrect ID");
            CollectionAssert.AreEqual(expectedMatrix1, matrix, "ALLOCATION-ID 2 is incorrect in Test1.tan");

        }

        [TestMethod]
        public void AllocationID3Test()
        {
            //Arrange
            int expectedID = 3;
            double[,] expectedMatrix1 = { { 1, 0, 0, 1, 0 }, { 0, 1, 1, 0, 0 }, { 0, 0, 0, 0, 1 } };

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            int id = test.SetOfAllocations[2].ID;
            double[,] matrix = test.SetOfAllocations[2].AllocationMatrix;

            //Assert
            Assert.AreEqual(expectedID, id, "ALLOCATION-ID 3 has incorrect ID");
            CollectionAssert.AreEqual(expectedMatrix1, matrix, "ALLOCATION-ID 3 is incorrect in Test1.tan");

        }

        [TestMethod]
        public void AllocationID4Test()
        {
            //Arrange
            int expectedID = 4;
            double[,] expectedMatrix1 = { { 1, 0, 0, 1, 0 }, { 0, 0, 0, 0, 1 }, { 0, 1, 1, 0, 0 } };

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            int id = test.SetOfAllocations[3].ID;
            double[,] matrix = test.SetOfAllocations[3].AllocationMatrix;

            //Assert
            Assert.AreEqual(expectedID, id, "ALLOCATION-ID 4 has incorrect ID");
            CollectionAssert.AreEqual(expectedMatrix1, matrix, "ALLOCATION-ID 4 is incorrect in Test1.tan");

        }

        [TestMethod]
        public void AllocationID5Test()
        {
            //Arrange
            int expectedID = 5;
            double[,] expectedMatrix1 = { { 0, 1, 0, 1, 0 }, { 1, 0, 1, 0, 0 }, { 0, 0, 0, 0, 1 } };

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            int id = test.SetOfAllocations[4].ID;
            double[,] matrix = test.SetOfAllocations[4].AllocationMatrix;

            //Assert
            Assert.AreEqual(expectedID, id, "ALLOCATION-ID 5 has incorrect ID");
            CollectionAssert.AreEqual(expectedMatrix1, matrix, "ALLOCATION-ID 5 is incorrect in Test1.tan");

        }

        [TestMethod]
        public void AllocationID6Test()
        {
            //Arrange
            int expectedID = 6;
            double[,] expectedMatrix1 = { { 0, 0, 1, 0, 0 }, { 1, 1, 0, 1, 0 }, { 0, 0, 0, 0, 1 } };

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            int id = test.SetOfAllocations[5].ID;
            double[,] matrix = test.SetOfAllocations[5].AllocationMatrix;

            //Assert
            Assert.AreEqual(expectedID, id, "ALLOCATION-ID 5 has incorrect ID");
            CollectionAssert.AreEqual(expectedMatrix1, matrix, "ALLOCATION-ID 5 is incorrect in Test1.tan");

        }

        [TestMethod]
        public void AllocationID7Test()
        {
            //Arrange
            int expectedID = 7;
            double[,] expectedMatrix1 = { { 0, 1, 0, 1, 0 }, { 0, 0, 0, 0, 1 }, { 1, 0, 1, 0, 0 } };

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            int id = test.SetOfAllocations[6].ID;
            double[,] matrix = test.SetOfAllocations[6].AllocationMatrix;

            //Assert
            Assert.AreEqual(expectedID, id, "ALLOCATION-ID 6 has incorrect ID");
            CollectionAssert.AreEqual(expectedMatrix1, matrix, "ALLOCATION-ID 6 is incorrect in Test1.tan");

        }

        [TestMethod]
        public void AllocationID8Test()
        {
            //Arrange
            int expectedID = 8;
            double[,] expectedMatrix1 = { { 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 1 }, { 1, 1, 0, 1, 0 } };

            //Act
            TaskAllocations test = new TaskAllocations(test1FilePath);
            TaskAllocations.TryParse(test1FilePath, out test);
            int id = test.SetOfAllocations[7].ID;
            double[,] matrix = test.SetOfAllocations[7].AllocationMatrix;

            //Assert
            Assert.AreEqual(expectedID, id, "ALLOCATION-ID 7 has incorrect ID");
            CollectionAssert.AreEqual(expectedMatrix1, matrix, "ALLOCATION-ID 7 is incorrect in Test1.tan");

        }
    }

    [TestClass]
    public class ConfigurationTests
    {
        //csv tests
        [TestMethod]
        public void ValidConfigurationTest()
        {
            //Arrange
            string configPath = "C:\\Users\\1530\\source\\repos\\SIT323AssessmentTask2\\Files for Unit Testing\\Test1.csv";
            bool expected = true;

            //Act
            Configuration.TryParse(configPath, out Configuration aConfiguration);
            bool actual = aConfiguration.Validate();

            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void InvalidConfigurationTest1()
        {
            //Arrange
            string configPath = "C:\\Users\\Tyson\\source\\repos\\SIT323Assignment1\\Files for Unit Testing\\Test3.csv";
            bool expected = false;

            //Act
            Configuration.TryParse(configPath, out Configuration aConfiguration);
            bool actual = aConfiguration.Validate();

            //Assert
            Assert.AreEqual(expected, actual);

        }
    }

    [TestClass]
    public class AllocationTests
    {
        //Allocation tests
        [TestMethod]
        public void validAllocationTest()
        {
            //Arrange
            int ID = 1;
            double[,] allocation = { { 1, 1, 0, 0, 0 }, { 0, 0, 1, 1, 0 }, { 0, 0, 0, 0, 1 } };
            bool expected = true;

            //Act
            Allocation anAllocation = new Allocation(ID, allocation);
            bool actual = anAllocation.ValidateAllocation(out List<string> errors);

            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void invalidAllocationTest1()
        {
            //Arrange
            int ID = 1;
            double[,] allocation = { { 1, 1, 0, 0, 0 }, { 0, 0, 1, 1, 0 }, { 0, 0, 0, 0, 0 } };
            bool expected = false;

            //Act
            Allocation anAllocation = new Allocation(ID, allocation);
            bool actual = anAllocation.ValidateAllocation(out List<string> errors);

            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void invalidAllocationTest2()
        {
            //Arrange
            int ID = 1;
            double[,] allocation = { { 1, 1, 0, 0, 0 }, { 0, 1, 1, 0, 0 }, { 0, 0, 1, 1, 0 } };
            bool expected = false;

            //Act
            Allocation anAllocation = new Allocation(ID, allocation);
            bool actual = anAllocation.ValidateAllocation(out List<string> errors);

            //Assert
            Assert.AreEqual(expected, actual);

        }
    }

    [TestClass]
    public class AllocationEnergyTests
    {
        //computing the energy consumed by an Allocation tests
        [TestMethod]
        public void AllocationEnergyTest1()
        {
            //Arrange
            string tanPath = "C:\\Users\\1530\\source\\repos\\SIT323AssessmentTask2\\Files for Unit Testing\\Test1.tan";
            string configPath = "C:\\Users\\1530\\source\\repos\\SIT323AssessmentTask2\\Files for Unit Testing\\Test1.csv";
            double expectedEnergy = 155.77;
            List<string> errors = new List<string>();

            //Act
            TaskAllocations.TryParse(tanPath, out TaskAllocations aTaskAllocation);
            Configuration.TryParse(configPath, out Configuration aConfiguration);
            aTaskAllocation.SetOfAllocations[0].CalculateTime(aConfiguration, out errors);
            double actualEnergy = Math.Round(aTaskAllocation.SetOfAllocations[0].CalculateEnergy(aConfiguration, out errors), 2);

            //Assert
            Assert.AreEqual(expectedEnergy, actualEnergy);

        }
    }

    [TestClass]
    public class AllocationRuntimeTests
    {
        //computing the runtime of an Allocation tests
        [TestMethod]
        public void AllocationRuntimeTest1()
        {
            //Arrange
            string tanPath = "C:\\Users\\1530\\source\\repos\\SIT323AssessmentTask2\\Files for Unit Testing\\Test1.tan";
            string configPath = "C:\\Users\\1530\\source\\repos\\SIT323AssessmentTask2\\Files for Unit Testing\\Test1.csv";
            double expectedTime = 2.61;
            List<string> errors = new List<string>();

            //Act
            TaskAllocations.TryParse(tanPath, out TaskAllocations aTaskAllocation);
            Configuration.TryParse(configPath, out Configuration aConfiguration);
            double actualtime = Math.Round(aTaskAllocation.SetOfAllocations[0].CalculateTime(aConfiguration, out errors), 2);

            //Assert
            Assert.AreEqual(expectedTime, actualtime);

        }
    }
}

