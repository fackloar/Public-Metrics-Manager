using MetricsManager;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL;


namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ILogger<AgentsController> _logger;
        private readonly IAgentsRepository _repository;

        public AgentsController(ILogger<AgentsController> logger, IAgentsRepository repository)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog is embed into AgentsController");
            _repository = repository;
        }
        /// <summary>
        /// Регистрирует нового агента по заданной информации
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     POST register/
        ///     
        /// <example>
        /// from BODY:
        /// <code>
        /// { 
        ///     "AgentUrl": "http://localhost:5001"
        /// }
        /// </code>
        /// </example>
        ///
        /// </remarks>
        /// <param name="agent">Agent</param>
        [HttpPost("register/{agent}")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agent)
        {
            _repository.Register(agent);
            return Ok();
        }
        /// <summary>
        /// Получает список зарегестрированных агентов
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     GET listAgents/
        ///
        /// </remarks>
        /// <returns>Список агентов</returns>
        [HttpGet("listAgents")]
        public IActionResult ListRegisteredAgents()
        {
            var listAgents = _repository.GetAgents();
            return Ok();
        }
    }
}
