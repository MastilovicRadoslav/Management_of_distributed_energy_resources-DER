using Common.Interfaces;
using Common.Models;
using DERClient.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace DERManagementSystem.Tests
{
    [TestClass]
    public class DERClientTests
    {
        private Mock<IDERService> _mockService;
        private DERClientService _clientService;

        [TestInitialize]
        public void Setup()
        {
            // Kreiranje mock IDERService objekta za testiranje
            _mockService = new Mock<IDERService>();
            _clientService = new DERClientService(_mockService.Object);
        }

        [TestMethod]
        public void TestResourceExists_ResourceExists_ReturnsTrue()
        {
            // Test proverava da li metod ResourceExists vraća true kada resurs postoji
            var resourceId = 1;
            var resources = new List<ResourceInfo> { new ResourceInfo { Id = resourceId, Name = "Resource1", Power = 25, IsActive = true } };
            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);

            var exists = _clientService.ResourceExists(resourceId);

            Assert.IsTrue(exists, "Expected resource to exist.");
        }

        [TestMethod]
        public void TestResourceExists_ResourceDoesNotExist_ReturnsFalse()
        {
            // Test proverava da li metod ResourceExists vraća false kada resurs ne postoji
            var resources = new List<ResourceInfo>();
            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);

            var exists = _clientService.ResourceExists(1);

            Assert.IsFalse(exists, "Expected resource to not exist.");
        }

        [TestMethod]
        public void TestDisplayResourceSchedule_ResourceExists_DisplaysInformation()
        {
            // Test proverava da li DisplayResourceSchedule prikazuje tačne informacije za postojeći resurs
            var resourceId = 1;
            var resources = new List<ResourceInfo>
            {
                new ResourceInfo { Id = resourceId, Name = "Resource1", Power = 25, IsActive = true, StartTime = DateTime.Now.AddHours(-1), ActiveTime = 3600 }
            };
            var statistics = new Statistics { TotalActivePower = 25, TotalProducedEnergy = 1.0 };
            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);
            _mockService.Setup(s => s.GetStatistics()).Returns(statistics);

            var result = _clientService.DisplayResourceSchedule(resourceId);

            Assert.IsTrue(result, "Expected DisplayResourceSchedule to return true for existing resource.");
        }

        [TestMethod]
        public void TestDisplayResourceSchedule_ResourceDoesNotExist_DisplaysError()
        {
            // Test proverava da li DisplayResourceSchedule vraća false za nepostojeći resurs
            var resources = new List<ResourceInfo>();
            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);

            var result = _clientService.DisplayResourceSchedule(1);

            Assert.IsFalse(result, "Expected DisplayResourceSchedule to return false for non-existent resource.");
        }

        [TestMethod]
        public void TestRegisterResource_ValidResource_ReturnsSuccessMessage()
        {
            // Test proverava da li RegisterResource vraća odgovarajuću poruku kada je resurs validan
            var resourceId = 1;
            _mockService.Setup(s => s.RegisterResource(resourceId)).Returns("Resource registered successfully.");

            var result = _clientService.RegisterResource(resourceId);

            Assert.AreEqual("Resource registered successfully.", result);
        }

        [TestMethod]
        public void TestUnregisterResource_ValidResource_DisplaysUpdatedSchedule()
        {
            // Test proverava da li UnregisterResource poziva ažuriranje resursa i prikazuje odgovarajuće informacije
            var resourceId = 1;
            var resources = new List<ResourceInfo> { new ResourceInfo { Id = resourceId, Name = "Resource1", Power = 25, IsActive = false, StartTime = DateTime.Now.AddHours(-1), ActiveTime = 3600 } };
            var statistics = new Statistics { TotalActivePower = 0, TotalProducedEnergy = 1.0 };
            _mockService.Setup(s => s.UnregisterResource(resourceId));
            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);
            _mockService.Setup(s => s.GetStatistics()).Returns(statistics);

            _clientService.UnregisterResource(resourceId);

            _mockService.Verify(s => s.UnregisterResource(resourceId), Times.Once);
        }
    }
}
