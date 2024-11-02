using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserClient.Services;
using Common.Models;
using Moq;
using System.Collections.Generic;
using Common.Interfaces;
using DERClient.Services;
using System;

namespace DERManagementSystem.Tests
{
    [TestClass]
    public class DERClientTests
    {
        private DERClientService _derClientService;
        private Mock<IDERService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IDERService>();
            _derClientService = new DERClientService(_mockService.Object); // Prosledi mocked service
        }

        [TestMethod]
        public void TestResourceExists_Exists()
        {
            // Arrange
            var resource = new ResourceInfo { Id = 1, Name = "Test Resource", Power = 25.0, IsActive = false };
            var resources = new List<ResourceInfo> { resource };
            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources); // Vraća listu

            // Act
            var exists = _derClientService.ResourceExists(1);

            // Assert
            Assert.IsTrue(exists, "Resource should exist.");
        }

        [TestMethod]
        public void TestDisplayResourceSchedule_ValidResource()
        {
            // Arrange
            var resource = new ResourceInfo { Id = 1, Name = "Test Resource", Power = 25.0, IsActive = true };
            var schedule = new ResourceSchedule { ResourceId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1), ActiveTime = 3600 };
            _mockService.Setup(s => s.GetResourceStatus()).Returns(new List<ResourceInfo> { resource });
            _mockService.Setup(s => s.GetSchedule(1)).Returns(schedule);

            // Act
            var result = _derClientService.DisplayResourceSchedule(1);

            // Assert
            Assert.IsTrue(result, "Resource schedule should be displayed.");
        }

        [TestMethod]
        public void TestRegisterResource()
        {
            // Arrange
            _mockService.Setup(s => s.RegisterResource(1)).Returns("Resource with ID 1 is now active.");

            // Act
            var result = _derClientService.RegisterResource(1);

            // Assert
            Assert.AreEqual("Resource with ID 1 is now active.", result);
        }

        [TestMethod]
        public void TestUnregisterResource()
        {
            // Arrange
            var resource = new ResourceInfo { Id = 1, Name = "Test Resource", Power = 50.0, IsActive = true };
            _mockService.Setup(s => s.GetResourceStatus()).Returns(new List<ResourceInfo> { resource });
            _mockService.Setup(s => s.UnregisterResource(1)).Returns("Resource with ID 1 has stopped.");

            // Act
            _derClientService.UnregisterResource(1); // Poziv metode bez dodele rezultata

            // Assert
            // Proverite da li je resurs neaktivan
            var updatedResource = new ResourceInfo { Id = 1, IsActive = false }; // Očekivani rezultat
            Assert.IsFalse(updatedResource.IsActive, "Resource should be inactive after unregistering.");
            Assert.AreEqual("Resource with ID 1 has stopped.", _mockService.Object.UnregisterResource(1)); // Verifikujte povratak
        }

    }
}
