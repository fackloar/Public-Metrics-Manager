using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Moq;
using MetricsManager.DAL;
using AutoMapper;
using MetricsManager;
using System.Collections.Generic;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private AgentsController controller;
        private Mock<ILogger<AgentsController>> mockLogger;
        private Mock<IAgentsRepository> mockRepository;


        public AgentsControllerUnitTests()
        {
            mockRepository = new Mock<IAgentsRepository>();
            mockLogger = new Mock<ILogger<AgentsController>>();
            controller = new AgentsController(mockLogger.Object, mockRepository.Object);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //moq
            mockRepository.Setup(repository => repository.GetAgents()).Returns(new List<AgentInfo>());

            //Arrange

            //Act
            var result = controller.ListRegisteredAgents();

            // Assert
            mockRepository.Verify(repository => repository.GetAgents());
        }
    }
}
