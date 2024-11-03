using Common.Models;
using DERServer.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace DERManagementSystem.Tests
{
    [TestClass]
    public class CommonTests
    {
        private const string TestFilePath = "C:\\Users\\Lenovo\\Desktop\\Zadnje\\Results\\test_resources.xml";
        private XmlDataAccess _xmlDataAccess;

        [TestInitialize]
        public void Setup()
        {
            _xmlDataAccess = new XmlDataAccess(TestFilePath);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath); // Očisti test fajl
            }
        }

        [TestMethod]
        public void TestDERResourceInitialization()
        {
            // Arrange
            var resource = new DERResource
            {
                Id = 1,
                Name = "Solar Panel 1",
                Power = 25.0,
                IsActive = false
            };

            // Assert
            Assert.AreEqual(1, resource.Id, "Resource ID should be 1.");
            Assert.AreEqual("Solar Panel 1", resource.Name, "Resource name should match.");
            Assert.AreEqual(25.0, resource.Power, "Resource power should match.");
            Assert.IsFalse(resource.IsActive, "Resource should be inactive by default.");
        }

        [TestMethod]
        public void TestSaveAndLoadResources()
        {
            // Arrange
            var resources = new List<DERResource>
            {
                new DERResource { Id = 1, Name = "Solar Panel 1", Power = 25.0, IsActive = false },
                new DERResource { Id = 2, Name = "Wind Turbine 1", Power = 50.0, IsActive = false }
            };

            // Act
            _xmlDataAccess.SaveResources(resources, new Statistics()); // Sačuvaj resurse
            var loadedResources = _xmlDataAccess.LoadResources(); // Učitaj resurse

            // Assert
            Assert.AreEqual(2, loadedResources.Count, "Should load two resources.");
            Assert.AreEqual("Solar Panel 1", loadedResources[0].Name, "First resource name should match.");
            Assert.AreEqual("Wind Turbine 1", loadedResources[1].Name, "Second resource name should match.");
        }

        [TestMethod]
        public void TestLoadResources_EmptyFile()
        {
            // Arrange
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath); // Očisti test fajl pre testa
            }

            // Act
            var loadedResources = _xmlDataAccess.LoadResources(); // Učitaj resurse

            // Assert
            Assert.AreEqual(0, loadedResources.Count, "Should return an empty resource list.");
        }

        [TestMethod]
        public void TestCreateEmptyXmlFile()
        {
            // Act
            var xmlDataAccess = new XmlDataAccess(TestFilePath);

            // Assert
            Assert.IsTrue(File.Exists(TestFilePath), "XML file should be created.");
            var resources = xmlDataAccess.LoadResources();
            Assert.AreEqual(0, resources.Count, "Should return an empty resource list.");
        }
    }
}
