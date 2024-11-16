using Common.Interfaces;
using Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using UserClient.Services;

namespace DERManagementSystem.Tests
{
    [TestClass]
    public class UserClientTests
    {
        private UserClientService _userService;
        private Mock<IDERService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            // Inicijalizacija mock IDERService i UserClientService
            _mockService = new Mock<IDERService>();
            _userService = new UserClientService(_mockService.Object);

            // Kreiranje testnog fajla za LoadAndRegisterResourcesFromFile
            string testData = "Name: Resource1\nPower: 30\nStatus: Inactive\n---\n" +
                  "Name: Resource2\nPower: 40\nStatus: Inactive\n---";
            File.WriteAllText("testFilePath.txt", testData);
        }

        [TestMethod]
        public void TestRegisterNewResource_AddsResourceSuccessfully()
        {
            // Test proverava da li RegisterNewResource dodaje resurs uspešno
            var resource = new DERResource { Name = "Test Resource", Power = 25, IsActive = false };
            _mockService.Setup(s => s.RegisterNewResource(It.IsAny<DERResource>())).Returns(resource);

            bool result = _userService.RegisterNewResource(resource);

            Assert.IsTrue(result, "Resource should be added successfully.");
            _mockService.Verify(s => s.RegisterNewResource(It.IsAny<DERResource>()), Times.Once);
        }

        [TestMethod]
        public void TestRegisterNewResource_DuplicateResource()
        {
            // Test proverava da li RegisterNewResource odbija duplikat resursa
            var resource = new DERResource { Name = "Duplicate Resource", Power = 25, IsActive = false };
            _mockService.Setup(s => s.RegisterNewResource(It.IsAny<DERResource>())).Returns((DERResource)null);

            bool result = _userService.RegisterNewResource(resource);

            Assert.IsFalse(result, "Duplicate resource should not be added.");
            _mockService.Verify(s => s.RegisterNewResource(It.IsAny<DERResource>()), Times.Once);
        }

        [TestMethod]
        public void TestLoadAndRegisterResourcesFromFile_AddsMultipleResources()
        {
            // Test proverava da li LoadAndRegisterResourcesFromFile dodaje više resursa iz fajla
            var resources = new List<DERResource>
            {
                new DERResource { Name = "Resource1", Power = 30 },
                new DERResource { Name = "Resource2", Power = 40 }
            };
            _mockService.Setup(s => s.RegisterNewResource(It.IsAny<DERResource>()))
                        .Returns((DERResource r) => r);

            _userService.LoadAndRegisterResourcesFromFile("testFilePath.txt");

            _mockService.Verify(s => s.RegisterNewResource(It.IsAny<DERResource>()), Times.Exactly(resources.Count));
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Brisanje testnog fajla nakon testova
            if (File.Exists("testFilePath.txt"))
            {
                File.Delete("testFilePath.txt");
            }
        }

        [TestMethod]
        public void TestDisplayResourceStatus_DisplaysAllResources()
        {
            // Test proverava da li DisplayResourceStatus ispravno prikazuje sve resurse
            var resources = new List<ResourceInfo>
            {
                new ResourceInfo { Id = 1, Name = "Resource1", Power = 25, IsActive = true },
                new ResourceInfo { Id = 2, Name = "Resource2", Power = 50, IsActive = false }
            };
            var statistics = new Statistics { TotalActivePower = 25, TotalProducedEnergy = 100 };

            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);
            _mockService.Setup(s => s.GetStatistics()).Returns(statistics);

            _userService.DisplayResourceStatus();

            _mockService.Verify(s => s.GetResourceStatus(), Times.Once);
            _mockService.Verify(s => s.GetStatistics(), Times.Once);
        }

        [TestMethod]
        public void TestClearAllResources_DeletesAllResources()
        {
            // Test proverava da li ClearAllResources uspešno briše sve resurse
            _userService.ClearAllResources();

            _mockService.Verify(s => s.ClearAllResources(), Times.Once);
        }
    }
}
