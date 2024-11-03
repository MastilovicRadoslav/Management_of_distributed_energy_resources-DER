using Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DERManagementSystem.Tests
{
    [TestClass]
    public class CommonTests
    {
        [TestMethod]
        public void TestDERResourceInitialization()
        {
            // Arrange
            var name = "Solar Panel";
            var power = 25.0;

            // Act
            var resource = new DERResource
            {
                Name = name,
                Power = power,
                IsActive = false
            };

            // Assert
            Assert.AreEqual(name, resource.Name, "Name should be set correctly.");
            Assert.AreEqual(power, resource.Power, "Power should be set correctly.");
            Assert.IsFalse(resource.IsActive, "Resource should be initialized as inactive.");
        }

        [TestMethod]
        public void TestDERResourceActivation()
        {
            // Arrange
            var resource = new DERResource
            {
                Name = "Wind Turbine",
                Power = 40,
                IsActive = false
            };

            // Act
            resource.IsActive = true;

            // Assert
            Assert.IsTrue(resource.IsActive, "Resource should be activated.");
        }

        [TestMethod]
        public void TestStatisticsInitialization()
        {
            // Arrange & Act
            var stats = new Statistics
            {
                TotalActivePower = 0,
                TotalProducedEnergy = 0
            };

            // Assert
            Assert.AreEqual(0, stats.TotalActivePower, "Total Active Power should initialize to 0.");
            Assert.AreEqual(0, stats.TotalProducedEnergy, "Total Produced Energy should initialize to 0.");
        }

        [TestMethod]
        public void TestStatisticsUpdate()
        {
            // Arrange
            var stats = new Statistics
            {
                TotalActivePower = 50,
                TotalProducedEnergy = 100
            };

            // Act
            stats.TotalActivePower += 20;
            stats.TotalProducedEnergy += 50;

            // Assert
            Assert.AreEqual(70, stats.TotalActivePower, "Total Active Power should update correctly.");
            Assert.AreEqual(150, stats.TotalProducedEnergy, "Total Produced Energy should update correctly.");
        }
    }
}
