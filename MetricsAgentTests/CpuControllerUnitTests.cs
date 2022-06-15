using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;
using AutoMapper;
using MetricsAgent;
using System.Collections.Generic;

namespace MetricsAgentTests
{
    public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsController controller;
        private Mock<ICpuMetricsRepository> mockRepository;
        private Mock<ILogger<CpuMetricsController>> mockLogger;

        public CpuMetricsControllerUnitTests()
        {
            mockRepository = new Mock<ICpuMetricsRepository>();
            mockLogger = new Mock<ILogger<CpuMetricsController>>();
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            controller = new CpuMetricsController(mockLogger.Object, mockRepository.Object, mapper);
        }

        [Fact]
        public void Create_ShouldCall_GetByTimePeriod_From_Repository()
        {
            //moq
            mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).Returns(new List<CpuMetric>());

            //Arrange
            var fromTime = DateTimeOffset.Now.AddDays(-10);
            var toTime = DateTimeOffset.Now;
            

            //Act
            var result = controller.GetByTimePeriod(fromTime, toTime);

            //Assert
            mockRepository.Verify(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()), Times.AtMostOnce());
        }
    }
}
