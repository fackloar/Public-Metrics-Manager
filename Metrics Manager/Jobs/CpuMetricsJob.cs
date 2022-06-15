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
    public class CpuMetricsJob : IJob
    {
        private ICpuMetricsRepository _repository;
        private readonly IAgentsRepository _agentsRepository;
        private readonly IMetricsAgentClient _metricsAgentClient;
        private readonly ILogger<CpuMetricsJob> _logger;

        public CpuMetricsJob(
            ICpuMetricsRepository repository,
            IAgentsRepository agentsRepository,
            IMetricsAgentClient metricsAgentClient,
            ILogger<CpuMetricsJob> logger)
        {
            _repository = repository;
            _agentsRepository = agentsRepository;
            _metricsAgentClient = metricsAgentClient;
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("CpuMetricsJob starts");

            var allAgents = _agentsRepository.GetAgents();

            foreach (var agent in allAgents)
            {
                var fromTime = _repository.GetLastTime(agent.AgentId);
                var toTime = DateTimeOffset.UtcNow;

                var request = new GetAllCpuMetricsApiRequest
                {
                    Client = agent.AgentUrl.ToString(),
                    FromTime = fromTime,
                    ToTime = toTime
                };

                var cpuMetrics = _metricsAgentClient.GetAllCpuMetrics(request);

                foreach (var metric in cpuMetrics.Metrics)
                {
                    var newMetric = new CpuMetric
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
