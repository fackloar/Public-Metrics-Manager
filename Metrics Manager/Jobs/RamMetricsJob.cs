using System.Threading.Tasks;
using Quartz;
using System;
using MetricsManager.DAL.Repositories;
using MetricsManager.Client;
using MetricsManager.DAL.Requests;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL;

namespace MetricsManager.Jobs
{
    public class RamMetricsJob : IJob
    {
        private IRamMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsAgentClient _metricsAgentClient;
        private readonly ILogger<RamMetricsJob> _logger;

        public RamMetricsJob(
            IRamMetricsRepository repository,
            IAgentsRepository agentsRepository,
            IMetricsAgentClient metricsAgentClient,
            ILogger<RamMetricsJob> logger)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("RamMetricsJob starts");

            var allAgents = _agentsRepository.GetAgents();

            foreach (var agent in allAgents)
            {
                var fromTime = _repository.GetLastTime(agent.AgentId);
                var toTime = DateTimeOffset.UtcNow;

                var request = new GetAllRamMetricsApiRequest
                {
                    Client = agent.AgentUrl.ToString(),
                    FromTime = fromTime,
                    ToTime = toTime
                };

                var RamMetrics = _metricsAgentClient.GetAllRamMetrics(request);

                foreach (var metric in RamMetrics.Metrics)
                {
                    var newMetric = new RamMetric
                    {
                        AgentId = metric.AgentId,
                        Value = metric.Value,
                        Time = metric.Time.ToUnixTimeSeconds()
                    };

                    _repository.Create(newMetric);
                }
            }
            return Task.CompletedTask;
        }
    }
}
