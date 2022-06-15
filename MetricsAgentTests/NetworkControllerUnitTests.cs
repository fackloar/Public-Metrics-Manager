using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;
using AutoMapper;
using System.Collections.Generic;
using MetricsAgent;

namespace MetricsAgentTests
{
    public class NetworkMetricsControllerUnitTests
    {
        private NetworkMetricsController controller;
        private Mock<INetworkMetricsRepository> mockRepository;
        private Mock<ILogger<NetworkMetricsController>> mockLogger;
        private IMapper mapper;

        public NetworkMetricsControllerUnitTests()
        {
            mockRepository = new Mock<INetworkMetricsRepository>();
            mockLogger = new Mock<ILogger<NetworkMetricsController>>();
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            controller = new NetworkMetricsController(mockLogger.Object, mockRepository.Object, mapper);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            //moq
            mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).Returns(new List<NetworkMetric>());

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
