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
    public class HddMetricsJob : IJob
    {
        private IHddMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsAgentClient _metricsAgentClient;
        private readonly ILogger<HddMetricsJob> _logger;

        public HddMetricsJob(
            IHddMetricsRepository repository,
            IAgentsRepository agentsRepository,
            IMetricsAgentClient metricsAgentClient,
            ILogger<HddMetricsJob> logger)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("HddMetricsJob starts");

            var allAgents = _agentsRepository.GetAgents();

            foreach (var agent in allAgents)
            {
                var fromTime = _repository.GetLastTime(agent.AgentId);
                var toTime = DateTimeOffset.UtcNow;

                var request = new GetAllHddMetricsApiRequest
                {
                    Client = agent.AgentUrl.ToString(),
                    FromTime = fromTime,
                    ToTime = toTime
                };

                var HddMetrics = _metricsAgentClient.GetAllHddMetrics(request);

                foreach (var metric in HddMetrics.Metrics)
                {
                    var newMetric = new HddMetric
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
