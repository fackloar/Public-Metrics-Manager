using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Responses
{
    public class AllDotNetMetricsApiResponse
    {
        public List<DotNetMetricDto> Metrics { get; set; }

        public class DotNetMetricDto
        {
            public DateTimeOffset Time { get; set; }
            public int Value { get; set; }
            public int Id { get; set; }
            public int AgentId { get; set; }

        }
    }
}
