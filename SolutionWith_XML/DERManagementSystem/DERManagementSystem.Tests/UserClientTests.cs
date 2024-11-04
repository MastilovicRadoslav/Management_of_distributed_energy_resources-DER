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
        private UserClientService _userClientService;
        private Mock<IDERService> _mockService;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IDERService>();
            _userClientService = new UserClientService(_mockService.Object); // Prosledi mocked service
        }

        [TestMethod]
        public void TestDisplayResourceStatus()
        {
            // Arrange
            var resources = new List<ResourceInfo>
            {
                new ResourceInfo { Id = 1, Name = "Test Resource 1", Power = 25.0, IsActive = true },
                new ResourceInfo { Id = 2, Name = "Test Resource 2", Power = 15.0, IsActive = false }
            };
            _mockService.Setup(s => s.GetResourceStatus()).Returns(resources);

            // Act
            _userClientService.DisplayResourceStatus();

            // Assert
            // Verify that the display method worked correctly
            // For example, you could check console output if needed (requires capturing console output)
        }
    }
}
