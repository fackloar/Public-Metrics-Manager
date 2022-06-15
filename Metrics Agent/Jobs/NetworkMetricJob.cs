using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    [DisallowConcurrentExecution]
    public class NetworkMetricJob : IJob
    {
        private INetworkMetricsRepository _repository;

        // счетчик для метрики Network
        private PerformanceCounter _NetworkCounter;


        public NetworkMetricJob(INetworkMetricsRepository repository)
        {
            _repository = repository;
            _NetworkCounter = new PerformanceCounter("Network Interface", "Bytes Total/sec", "Realtek 8811CU Wireless LAN 802.11ac USB NIC");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости Network
            var NetworkBytesPerSec = Convert.ToInt32(_NetworkCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new NetworkMetric { Time = time, Value = NetworkBytesPerSec });

            return Task.CompletedTask;
        }
    }
}




