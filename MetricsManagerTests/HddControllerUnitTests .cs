using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using MetricsManager.DAL.Repositories;
using AutoMapper;
using MetricsManager;
using MetricsManager.DAL;
using System.Collections.Generic;

namespace MetricsManagerTests
{
    public class HddMetricsControllerUnitTests
    {
        private HddMetricsController controller;
        private Mock<IHddMetricsRepository> mockRepository;
        private Mock<ILogger<HddMetricsController>> mockLogger;

        public HddMetricsControllerUnitTests()
        {
            mockRepository = new Mock<IHddMetricsRepository>();
            mockLogger = new Mock<ILogger<HddMetricsController>>();
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            controller = new HddMetricsController(mockLogger.Object, mockRepository.Object, mapper);
        }

        [Fact]
        public void Create_ShouldCall_GetByTimePeriod_From_Repository()
        {
            //moq
            mockRepository.Setup(repository => repository.GetByAgentId(It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).Returns(new List<HddMetric>());

            //Arrange
            var fromTime = DateTimeOffset.Now.AddDays(-10);
            var toTime = DateTimeOffset.Now;
            var agentId = 1;


            //Act
            var result = controller.GetByTimePeriod(agentId, fromTime, toTime);

            //Assert
            mockRepository.Verify(repository => repository.GetByAgentId(It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.AtMostOnce());
        }
    }
}
