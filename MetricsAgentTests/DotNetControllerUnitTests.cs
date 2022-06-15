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
    public class DotNetMetricsControllerUnitTests
    {
        private DotNetMetricsController controller;
        private Mock<IDotNetMetricsRepository> mockRepository;
        private Mock<ILogger<DotNetMetricsController>> mockLogger;
        private IMapper mapper;

        public DotNetMetricsControllerUnitTests()
        {
            mockRepository = new Mock<IDotNetMetricsRepository>();
            mockLogger = new Mock<ILogger<DotNetMetricsController>>();
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            controller = new DotNetMetricsController(mockLogger.Object, mockRepository.Object, mapper);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            //moq
            mockRepository.Setup(repository => repository.GetByTimePeriod(It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>())).Returns(new List<DotNetMetric>());

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
