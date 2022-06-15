using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Responses
{
    public class AllRamMetricsApiResponse
    {
        public List<RamMetricDto> Metrics { get; set; }

        public class RamMetricDto
        {
            public DateTimeOffset Time { get; set; }
            public int Value { get; set; }
            public int Id { get; set; }
            public int AgentId { get; set; }

        }
    }
}
