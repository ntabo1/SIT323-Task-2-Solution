using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIT323AssessmentTask2;

namespace SIT323AssessmentTask2Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            TaskAllocations test = new TaskAllocations("Test1.tan");

            //Act
            TaskAllocations.TryParse("Test1.tan", out test);
            //Assert

        }
    }
}
