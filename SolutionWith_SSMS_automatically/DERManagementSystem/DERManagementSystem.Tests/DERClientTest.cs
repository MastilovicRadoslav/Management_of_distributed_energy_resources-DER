using Common.Interfaces;
using Common.Models;
using DERClient.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

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
            // Kreiramo mock IDERService objekat za testiranje
            _mockService = new Mock<IDERService>();
            _clientService = new DERClientService(_mockService.Object);
        }

        [TestMethod]
        public void TestActivateAndDeactivateRandomResource_NoResources_DoesNotThrowException()
        {
            // Test proverava da li metoda ne baca izuzetak kada nema resursa
            _mockService.Setup(s => s.GetResourceStatus()).Returns(new List<ResourceInfo>());

            _clientService.ActivateAndDeactivateRandomResource();

            // Ako nema izuzetaka, test je uspešan
            Assert.IsTrue(true, "Method should not throw an exception when no resources are present.");
        }

        [TestMethod]
        public void TestActivateAndDeactivateRandomResource_ActivatesAndDeactivatesResource()
        {
            // Test proverava da li se resurs nasumično aktivira i deaktivira
            var resourceId = 1;
            var resources = new List<ResourceInfo>
            {
                new ResourceInfo { Id = resourceId, Name = "Resource1", Power = 25, IsActive = false }
            };
            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);
            _mockService.Setup(s => s.RegisterResource(resourceId)).Returns($"Resource with ID {resourceId} is now active.");
            _mockService.Setup(s => s.UnregisterResource(resourceId));

            _clientService.ActivateAndDeactivateRandomResource();

            _mockService.Verify(s => s.RegisterResource(resourceId), Times.Once, "Resource should be activated once.");
            _mockService.Verify(s => s.UnregisterResource(resourceId), Times.Once, "Resource should be deactivated once.");
        }

        [TestMethod]
        public void TestActivateAndDeactivateRandomResource_WithMultipleResources_ActivatesOneRandomly()
        {
            // Test proverava da li se aktivira samo jedan nasumično odabrani resurs od više dostupnih
            var resources = new List<ResourceInfo>
            {
                new ResourceInfo { Id = 1, Name = "Resource1", Power = 25, IsActive = false },
                new ResourceInfo { Id = 2, Name = "Resource2", Power = 30, IsActive = false }
            };
            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);
            _mockService.Setup(s => s.RegisterResource(It.IsAny<int>())).Returns((int id) => $"Resource with ID {id} is now active.");
            _mockService.Setup(s => s.UnregisterResource(It.IsAny<int>()));

            _clientService.ActivateAndDeactivateRandomResource();

            // Proveravamo da li je tačno jedan resurs aktiviran i deaktiviran
            _mockService.Verify(s => s.RegisterResource(It.IsAny<int>()), Times.Once, "Only one resource should be activated.");
            _mockService.Verify(s => s.UnregisterResource(It.IsAny<int>()), Times.Once, "Only one resource should be deactivated.");
        }

    }
}
