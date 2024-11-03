using DERServer.Services;
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
            _service = new DERService(); // Inicijalizujte službu
            _service.ClearAllResources(); // Očistite resurse pre svakog testa
        }

        [TestMethod]
        public void TestRegisterNewResource()
        {
            // Arrange
            var resource = new DERResource { Id = 1, Name = "Test Resource", Power = 25, IsActive = false };

            // Act
            _service.RegisterNewResource(resource); // Poziv bez dodele rezultata

            // Assert
            var resources = _service.GetResourceStatus();
            Assert.IsTrue(resources.Any(r => r.Id == resource.Id), "Resource should be added.");
        }

        [TestMethod]
        public void TestRegisterDuplicateResource()
        {
            // Arrange
            var resource = new DERResource { Id = 1, Name = "Test Resource", Power = 25, IsActive = false };
            _service.RegisterNewResource(resource); // Registruj prvi put

            // Act
            _service.RegisterNewResource(resource); // Registruj ponovo

            // Assert
            var resources = _service.GetResourceStatus();
            Assert.AreEqual(1, resources.Count(r => r.Id == resource.Id), "Should not allow duplicate resource.");
        }


        [TestMethod]
        public void TestActivateResource()
        {
            // Arrange
            var resource = new DERResource { Id = 2, Name = "Active Resource", Power = 50, IsActive = false };
            _service.RegisterNewResource(resource);

            // Act
            var result = _service.RegisterResource(2);

            // Assert
            Assert.IsTrue(result.Contains("is now active"), "Resource should be activated.");
        }

        [TestMethod]
        public void TestActivateNonexistentResource()
        {
            // Act
            var result = _service.RegisterResource(999); // Attempt to activate a resource that doesn't exist

            // Assert
            Assert.IsTrue(result.Contains("Resource not found"), "Should return an error for non-existent resource.");
        }

        [TestMethod]
        public void TestUnregisterResource()
        {
            // Arrange
            var resource = new DERResource { Id = 1, Name = "Test Resource", Power = 50.0, IsActive = true };
            _service.RegisterNewResource(resource); // Registruj novi resurs
            _service.RegisterResource(resource.Id); // Aktiviraj resurs pre nego što ga deaktivišete

            // Act
            var result = _service.UnregisterResource(resource.Id);

            // Assert
            Assert.IsTrue(result.Contains("has stopped"), "Resource should be marked as stopped.");
            Assert.IsFalse(_service.GetResourceStatus().Any(r => r.Id == resource.Id && r.IsActive), "Resource should be inactive after unregistering.");
        }

        [TestMethod]
        public void TestUnregisterInactiveResource()
        {
            // Arrange
            var resource = new DERResource { Id = 2, Name = "Inactive Resource", Power = 50.0, IsActive = false };
            _service.RegisterNewResource(resource); // Registruj novi resurs

            // Act
            var result = _service.UnregisterResource(resource.Id);

            // Assert
            Assert.IsTrue(result.Contains("not active"), "Should return error for inactive resource.");
        }

        [TestMethod]
        public void TestClearAllResources()
        {
            // Arrange
            var resource1 = new DERResource { Id = 3, Name = "Resource To Clear", Power = 30, IsActive = false };
            var resource2 = new DERResource { Id = 4, Name = "Another Resource", Power = 20, IsActive = false };
            _service.RegisterNewResource(resource1);
            _service.RegisterNewResource(resource2);

            // Act
            _service.ClearAllResources();

            // Assert
            var resources = _service.GetResourceStatus();
            Assert.IsFalse(resources.Any(), "All resources should be cleared.");
        }

        [TestMethod]
        public void TestGetResourceStatus()
        {
            // Arrange
            var resource = new DERResource { Id = 5, Name = "Status Resource", Power = 25, IsActive = false };
            _service.RegisterNewResource(resource);

            // Act
            var resources = _service.GetResourceStatus();

            // Assert
            Assert.IsTrue(resources.Any(r => r.Id == resource.Id), "Should retrieve the resource status correctly.");
        }
    }
}
