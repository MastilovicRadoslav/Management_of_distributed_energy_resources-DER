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
            // Kreiramo mock IDERService objekat za testiranje za XML bazu
            _mockService = new Mock<IDERService>();
            _clientService = new DERClientService(_mockService.Object);
        }

        /// <summary>
        /// Testira ponašanje kada nema resursa u XML bazi.
        /// </summary>
        [TestMethod]
        public void TestActivateAndDeactivateRandomResource_NoResources_DoesNotThrowException()
        {
            // Arrange: Postavi da XML baza vraća praznu listu resursa
            _mockService.Setup(s => s.GetResourceStatus()).Returns(new List<ResourceInfo>());

            // Act: Poziva se metoda i proverava da li baca izuzetke
            _clientService.ActivateAndDeactivateRandomResource();

            // Assert: Ako nema izuzetaka, test je uspešan
            Assert.IsTrue(true, "Method should not throw an exception when no resources are present.");
        }

        /// <summary>
        /// Proverava da se nasumično odabrani resurs u XML bazi aktivira i deaktivira.
        /// </summary>
        [TestMethod]
        public void TestActivateAndDeactivateRandomResource_ActivatesAndDeactivatesResource()
        {
            // Arrange: Kreira listu sa jednim resursom i postavlja očekivanje za aktivaciju i deaktivaciju
            var resourceId = 1;
            var resources = new List<ResourceInfo>
            {
                new ResourceInfo { Id = resourceId, Name = "Resource1", Power = 25, IsActive = false }
            };
            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);
            _mockService.Setup(s => s.RegisterResource(resourceId)).Returns($"Resource with ID {resourceId} is now active.");
            _mockService.Setup(s => s.UnregisterResource(resourceId));

            // Act: Aktivira i deaktivira resurs
            _clientService.ActivateAndDeactivateRandomResource();

            // Assert: Proverava da li su metode za aktivaciju i deaktivaciju pozvane jednom
            _mockService.Verify(s => s.RegisterResource(resourceId), Times.Once, "Resource should be activated once.");
            _mockService.Verify(s => s.UnregisterResource(resourceId), Times.Once, "Resource should be deactivated once.");
        }

        /// <summary>
        /// Testira da se samo jedan resurs aktivira i deaktivira kada postoji više resursa u XML bazi.
        /// </summary>
        [TestMethod]
        public void TestActivateAndDeactivateRandomResource_WithMultipleResources_ActivatesOneRandomly()
        {
            // Arrange: Kreira listu sa dva resursa i postavlja očekivanja za nasumičnu aktivaciju i deaktivaciju jednog resursa
            var resources = new List<ResourceInfo>
            {
                new ResourceInfo { Id = 1, Name = "Resource1", Power = 25, IsActive = false },
                new ResourceInfo { Id = 2, Name = "Resource2", Power = 30, IsActive = false }
            };
            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);
            _mockService.Setup(s => s.RegisterResource(It.IsAny<int>())).Returns((int id) => $"Resource with ID {id} is now active.");
            _mockService.Setup(s => s.UnregisterResource(It.IsAny<int>()));

            // Act: Aktivira i deaktivira jedan nasumičan resurs
            _clientService.ActivateAndDeactivateRandomResource();

            // Assert: Proverava da li je tačno jedan resurs aktiviran i deaktiviran
            _mockService.Verify(s => s.RegisterResource(It.IsAny<int>()), Times.Once, "Only one resource should be activated.");
            _mockService.Verify(s => s.UnregisterResource(It.IsAny<int>()), Times.Once, "Only one resource should be deactivated.");
        }
    }
}
