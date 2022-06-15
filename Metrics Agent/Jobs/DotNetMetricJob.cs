using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    [DisallowConcurrentExecution]
    public class DotNetMetricJob : IJob
    {
        private IDotNetMetricsRepository _repository;

        // счетчик для метрики DotNet
        private PerformanceCounter _DotNetCounter;


        public DotNetMetricJob(IDotNetMetricsRepository repository)
        {
            _repository = repository;
            _DotNetCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all Heaps", "_Global_");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости DotNet
            var DotNetBytesInHeaps = Convert.ToInt32(_DotNetCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new DotNetMetric { Time = time, Value = DotNetBytesInHeaps });

            return Task.CompletedTask;
        }
    }
}




