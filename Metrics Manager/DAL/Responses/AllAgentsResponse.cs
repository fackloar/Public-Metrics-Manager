using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Responses
{
    public class AllAgentsResponse
    {
        public List<AgentInfo> Agents { get; set; }
    }
}
