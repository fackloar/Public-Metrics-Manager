using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    [DisallowConcurrentExecution]
    public class HddMetricJob : IJob
    {
        private IHddMetricsRepository _repository;

        // счетчик для метрики Hdd
        private PerformanceCounter _HddCounter;


        public HddMetricJob(IHddMetricsRepository repository)
        {
            _repository = repository;
            _HddCounter = new PerformanceCounter("PhysicalDisk", "Avg. Disk sec/Read", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости Hdd
            var HddAvgDiskRead = Convert.ToInt32(_HddCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new HddMetric { Time = time, Value = HddAvgDiskRead });

            return Task.CompletedTask;
        }
    }
}




