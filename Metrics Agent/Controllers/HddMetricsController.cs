using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using MetricsAgent.DAL;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using System.Collections.Generic;
using static MetricsAgent.Responses.AllHddMetricsResponse;
using AutoMapper;


namespace MetricsAgent.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private readonly ILogger<HddMetricsController> _logger;
        private IHddMetricsRepository repository;
        private readonly IMapper mapper;

        public HddMetricsController(ILogger<HddMetricsController> logger, IHddMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog is embed into HddMetricsController");
            this.repository = repository;
            this.mapper = mapper;
        }
        /// <summary>
        /// Получает метрики Hdd на заданном диапазоне времени
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET Hddmetrics/from/2021-04-30/to/2021-05-30
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метрика времени в формате даты</param>
        /// <param name="toTime">конечная метрика времени в формате даты</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="201">Если ОК</response>
        /// <response code="400">Неправильные параметры</response>  
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetByTimePeriod of HddMetrics in Agent\nfromTime = {fromTime}\ntoTime = {toTime}");
            var metrics = repository.GetByTimePeriod(fromTime, toTime);
            var response = new AllHddMetricsResponse()
            {
                Metrics = new List<HddMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<HddMetricDto>(metric));

            }
            return Ok();
        }
    }
}
