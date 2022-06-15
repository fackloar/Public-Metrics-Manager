using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    [DisallowConcurrentExecution]
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository _repository;

        // счетчик для метрики CPU
        private PerformanceCounter _ramCounter;


        public RamMetricJob(IRamMetricsRepository repository)
        {
            _repository = repository;
            _ramCounter = new PerformanceCounter("Memory", "Available Mbytes", null);
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var ramMbytes = Convert.ToInt32(_ramCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new RamMetric { Time = time, Value = ramMbytes });

            return Task.CompletedTask;
        }
    }
}




