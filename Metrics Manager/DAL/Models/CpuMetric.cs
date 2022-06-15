using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL
{
    public class CpuMetric
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public long Time { get; set; }
        public int AgentId { get; set; }

    }
}
