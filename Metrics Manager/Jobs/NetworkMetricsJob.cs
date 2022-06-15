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
    public class NetworkMetricsJob : IJob
    {
        private INetworkMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsAgentClient _metricsAgentClient;
        private readonly ILogger<NetworkMetricsJob> _logger;

        public NetworkMetricsJob(
            INetworkMetricsRepository repository,
            IAgentsRepository agentsRepository,
            IMetricsAgentClient metricsAgentClient,
            ILogger<NetworkMetricsJob> logger)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("NetworkMetricsJob starts");

            var allAgents = _agentsRepository.GetAgents();

            foreach (var agent in allAgents)
            {
                var fromTime = _repository.GetLastTime(agent.AgentId);
                var toTime = DateTimeOffset.UtcNow;

                var request = new GetAllNetworkMetricsApiRequest
                {
                    Client = agent.AgentUrl.ToString(),
                    FromTime = fromTime,
                    ToTime = toTime
                };

                var NetworkMetrics = _metricsAgentClient.GetAllNetworkMetrics(request);

                foreach (var metric in NetworkMetrics.Metrics)
                {
                    var newMetric = new NetworkMetric
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
