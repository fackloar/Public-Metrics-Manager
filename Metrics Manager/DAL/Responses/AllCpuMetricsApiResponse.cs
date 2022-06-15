using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Responses
{
    public class AllCpuMetricsApiResponse
    {
        public List<CpuMetricDto> Metrics { get; set; }

        public class CpuMetricDto
        {
            public DateTimeOffset Time { get; set; }
            public int Value { get; set; }
            public int Id { get; set; }
            public int AgentId { get; set; }
        }

    }
}
