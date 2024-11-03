using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Common.Interfaces;
using Common.Models;
using System.Collections.Generic;
using UserClient.Services;
using System.IO;

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
            _mockService = new Mock<IDERService>();

            // Kreiranje instance UserClientService sa mock-ovanim IDERService
            _userService = new UserClientService(_mockService.Object);

            string testData = "Name: Resource1\nPower: 30\nStatus: Inactive\n---\n" +
                  "Name: Resource2\nPower: 40\nStatus: Inactive\n---";
            File.WriteAllText("testFilePath.txt", testData);
        }


        [TestMethod]
        public void TestRegisterNewResource_AddsResourceSuccessfully()
        {
            // Arrange
            var resource = new DERResource { Name = "Test Resource", Power = 25, IsActive = false };
            _mockService.Setup(s => s.RegisterNewResource(It.IsAny<DERResource>())).Returns(resource);

            // Act
            bool result = _userService.RegisterNewResource(resource);

            // Assert
            Assert.IsTrue(result, "Resource should be added successfully.");
            _mockService.Verify(s => s.RegisterNewResource(It.IsAny<DERResource>()), Times.Once);
        }

        [TestMethod]
        public void TestRegisterNewResource_DuplicateResource()
        {
            // Arrange
            var resource = new DERResource { Name = "Duplicate Resource", Power = 25, IsActive = false };
            _mockService.Setup(s => s.RegisterNewResource(It.IsAny<DERResource>())).Returns((DERResource)null);

            // Act
            bool result = _userService.RegisterNewResource(resource);

            // Assert
            Assert.IsFalse(result, "Duplicate resource should not be added.");
            _mockService.Verify(s => s.RegisterNewResource(It.IsAny<DERResource>()), Times.Once);
        }

        [TestMethod]
        public void TestLoadAndRegisterResourcesFromFile_AddsMultipleResources()
        {
            // Arrange
            var resources = new List<DERResource>
            {
                new DERResource { Name = "Resource1", Power = 30 },
                new DERResource { Name = "Resource2", Power = 40 }
            };
            _mockService.Setup(s => s.RegisterNewResource(It.IsAny<DERResource>()))
                        .Returns((DERResource r) => r);

            // Act
            _userService.LoadAndRegisterResourcesFromFile("testFilePath.txt");

            // Assert
            _mockService.Verify(s => s.RegisterNewResource(It.IsAny<DERResource>()), Times.Exactly(resources.Count));
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Brisanje test fajla nakon testa
            if (File.Exists("testFilePath.txt"))
            {
                File.Delete("testFilePath.txt");
            }
        }

        [TestMethod]
        public void TestDisplayResourceStatus_DisplaysAllResources()
        {
            // Arrange
            var resources = new List<ResourceInfo>
            {
                new ResourceInfo { Id = 1, Name = "Resource1", Power = 25, IsActive = true },
                new ResourceInfo { Id = 2, Name = "Resource2", Power = 50, IsActive = false }
            };
            var statistics = new Statistics { TotalActivePower = 25, TotalProducedEnergy = 100 };

            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);
            _mockService.Setup(s => s.GetStatistics()).Returns(statistics);

            // Act
            _userService.DisplayResourceStatus();

            // Assert
            _mockService.Verify(s => s.GetResourceStatus(), Times.Once);
            _mockService.Verify(s => s.GetStatistics(), Times.Once);
        }

        [TestMethod]
        public void TestClearAllResources_DeletesAllResources()
        {
            // Act
            _userService.ClearAllResources();

            // Assert
            _mockService.Verify(s => s.ClearAllResources(), Times.Once);
        }
    }
}
