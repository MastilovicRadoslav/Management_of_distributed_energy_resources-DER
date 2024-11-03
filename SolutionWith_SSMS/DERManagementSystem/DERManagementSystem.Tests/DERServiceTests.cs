﻿using DERServer.Services;
using Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DERManagementSystem.Tests
{
    [TestClass]
    public class DERServiceTests
    {
        private DERService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new DERService();
            _service.ClearAllResources(); // Resetovanje baze pre svakog testa
        }

        [TestMethod]
        public void TestRegisterNewResource()
        {
            var resource = new DERResource { Name = "Test Resource", Power = 25, IsActive = false };

            var addedResource = _service.RegisterNewResource(resource);

            Assert.IsNotNull(addedResource, "Resource should be added.");
            Assert.AreNotEqual(0, addedResource.Id, "Resource ID should be generated by the database.");

            var resources = _service.GetResourceStatus();
            Assert.IsTrue(resources.Any(r => r.Id == addedResource.Id), "Resource should be present in the database.");
        }

        [TestMethod]
        public void TestRegisterDuplicateResource()
        {
            var resource = new DERResource { Name = "Test Resource", Power = 25, IsActive = false };
            _service.RegisterNewResource(resource);

            var duplicateResource = _service.RegisterNewResource(resource);

            Assert.IsNull(duplicateResource, "Should not allow duplicate resource.");
            var resources = _service.GetResourceStatus();
            Assert.AreEqual(1, resources.Count(r => r.Name == resource.Name), "Only one resource with this name should exist.");
        }

        [TestMethod]
        public void TestActivateResource()
        {
            var resource = new DERResource { Name = "Active Resource", Power = 50, IsActive = false };
            var addedResource = _service.RegisterNewResource(resource);

            var result = _service.RegisterResource(addedResource.Id);

            Assert.IsTrue(result.Contains("is now active"), "Resource should be activated.");
            var updatedResource = _service.GetResourceStatus().First(r => r.Id == addedResource.Id);
            Assert.IsTrue(updatedResource.IsActive, "Resource should be marked as active.");
        }

        [TestMethod]
        public void TestActivateNonexistentResource()
        {
            var result = _service.RegisterResource(999);

            Assert.IsTrue(result.Contains("Resource not found"), "Should return error for non-existent resource.");
        }

        [TestMethod]
        public void TestUnregisterResource()
        {
            var resource = new DERResource { Name = "Test Resource", Power = 50.0, IsActive = false };
            var addedResource = _service.RegisterNewResource(resource);
            _service.RegisterResource(addedResource.Id);

            var result = _service.UnregisterResource(addedResource.Id);

            Assert.IsTrue(result.Contains("has stopped"), "Resource should be marked as stopped.");
            var updatedResource = _service.GetResourceStatus().First(r => r.Id == addedResource.Id);
            Assert.IsFalse(updatedResource.IsActive, "Resource should be inactive after unregistering.");
        }

        [TestMethod]
        public void TestUnregisterInactiveResource()
        {
            var resource = new DERResource { Name = "Inactive Resource", Power = 50.0, IsActive = false };
            var addedResource = _service.RegisterNewResource(resource);

            var result = _service.UnregisterResource(addedResource.Id);

            Assert.IsTrue(result.Contains("not active"), "Should return error for inactive resource.");
        }

        [TestMethod]
        public void TestClearAllResources()
        {
            var resource1 = new DERResource { Name = "Resource To Clear", Power = 30, IsActive = false };
            var resource2 = new DERResource { Name = "Another Resource", Power = 20, IsActive = false };
            _service.RegisterNewResource(resource1);
            _service.RegisterNewResource(resource2);

            _service.ClearAllResources();

            var resources = _service.GetResourceStatus();
            Assert.IsFalse(resources.Any(), "All resources should be cleared.");
        }

        [TestMethod]
        public void TestGetResourceStatus()
        {
            var resource = new DERResource { Name = "Status Resource", Power = 25, IsActive = false };
            var addedResource = _service.RegisterNewResource(resource);

            var resources = _service.GetResourceStatus();

            Assert.IsTrue(resources.Any(r => r.Id == addedResource.Id), "Should retrieve the resource status correctly.");
        }

        [TestMethod]
        public void TestGetStatistics()
        {
            var resource1 = new DERResource { Name = "Stat Resource 1", Power = 30, IsActive = false };
            var resource2 = new DERResource { Name = "Stat Resource 2", Power = 20, IsActive = false };
            _service.RegisterNewResource(resource1);
            _service.RegisterNewResource(resource2);

            _service.RegisterResource(resource1.Id);
            _service.RegisterResource(resource2.Id);

            var statsBefore = _service.GetStatistics();
            _service.UnregisterResource(resource1.Id);
            _service.UnregisterResource(resource2.Id);
            var statsAfter = _service.GetStatistics();

            Assert.IsTrue(statsBefore.TotalActivePower > 0, "TotalActivePower should be greater than 0 when resources are active.");
            Assert.IsTrue(statsAfter.TotalActivePower == 0, "TotalActivePower should be 0 when all resources are inactive.");
            Assert.IsTrue(statsAfter.TotalProducedEnergy >= 0, "TotalProducedEnergy should be calculated.");
        }
    }
}