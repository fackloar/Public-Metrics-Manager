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
    public class HddMetricsControllerUnitTests
    {
        private HddMetricsController controller;
        private Mock<IHddMetricsRepository> mockRepository;
        private Mock<ILogger<HddMetricsController>> mockLogger;
        private IMapper mapper;

        public HddMetricsControllerUnitTests()
        {
            mockRepository = new Mock<IHddMetricsRepository>();
            mockLogger = new Mock<ILogger<HddMetricsController>>();
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            controller = new HddMetricsController(mockLogger.Object ,mockRepository.Object, mapper);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            //moq
            mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).Returns(new List<HddMetric>());

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
