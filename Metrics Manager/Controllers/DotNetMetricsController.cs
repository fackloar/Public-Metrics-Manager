using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using AutoMapper;
using MetricsManager.DAL.Repositories;
using MetricsManager.DAL.Responses;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/DotNet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsController> _logger;
        private IDotNetMetricsRepository repository;
        private readonly IMapper mapper;

        public DotNetMetricsController(ILogger<DotNetMetricsController> logger, IDotNetMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog is embed into DotNetMetricsController");
            this.repository = repository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получает метрики DotNet от агента по номеру на заданном диапазоне времени
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET DotNetmetrics/agent/1/from/2021-04-30/to/2021-05-30/
        ///
        /// </remarks>
        /// <param name="agentId">номер агента</param>
        /// <param name="fromTime">начальная метрика времени в формате даты</param>
        /// <param name="toTime">конечная метрика времени в формате даты</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени на заданном агенте</returns>
        /// <response code="201">Все ОК</response>
        /// <response code="400">переданы неправильные параметры</response>  
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod([FromRoute] int agentId, [FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetByTimePeriod of DotNetMetrics in Agent={agentId}\nfromTime = {fromTime}\ntoTime = {toTime}");
            var metrics = repository.GetByAgentId(agentId, fromTime, toTime);
            var response = new AllDotNetMetricsApiResponse()
            {
                Metrics = new List<AllDotNetMetricsApiResponse.DotNetMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<AllDotNetMetricsApiResponse.DotNetMetricDto>(metric));
            }
            return Ok();
        }
        /// <summary>
        /// Получает метрики DotNet на заданном диапазоне времени по всем агентам
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET DotNetmetrics/from/2021-04-30/to/2021-05-30
        ///
        /// </remarks>
        /// <param name="fromTime">начальная метрика времени в формате даты</param>
        /// <param name="toTime">конечная метрика времени в формате даты</param>
        /// <returns>Список метрик, которые были сохранены в заданном диапазоне времени</returns>
        /// <response code="201">Если ОК</response>
        /// <response code="400">Неправильные параметры</response>  
        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetByCluster([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"GetByCluster of DotNetMetrics\nfromTime = {fromTime}\ntoTime = {toTime}");

            var metrics = repository.GetByCluster(fromTime, toTime);
            var response = new AllDotNetMetricsApiResponse()
            {
                Metrics = new List<AllDotNetMetricsApiResponse.DotNetMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<AllDotNetMetricsApiResponse.DotNetMetricDto>(metric));
            }
            return Ok();
        }
    }
}
