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
    public class DotNetMetricsJob : IJob
    {
        private IDotNetMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsAgentClient _metricsAgentClient;
        private readonly ILogger<DotNetMetricsJob> _logger;

        public DotNetMetricsJob(
            IDotNetMetricsRepository repository,
            IAgentsRepository agentsRepository,
            IMetricsAgentClient metricsAgentClient,
            ILogger<DotNetMetricsJob> logger)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("DotNetMetricsJob starts");

            var allAgents = _agentsRepository.GetAgents();

            foreach (var agent in allAgents)
            {
                var fromTime = _repository.GetLastTime(agent.AgentId);
                var toTime = DateTimeOffset.UtcNow;

                var request = new DotNetHeapMetricsApiRequest
                {
                    Client = agent.AgentUrl.ToString(),
                    FromTime = fromTime,
                    ToTime = toTime
                };

                var DotNetMetrics = _metricsAgentClient.GetDotNetMetrics(request);

                foreach (var metric in DotNetMetrics.Metrics)
                {
                    var newMetric = new DotNetMetric
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
