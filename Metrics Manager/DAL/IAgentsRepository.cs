using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MetricsManager.DAL.Responses.AllAgentsResponse;

namespace MetricsManager.DAL
{
    public interface IAgentsRepository
    {
        public string GetAddressByAgentId(int agentId);
        public IList<AgentInfo> GetAgents();
        public void Register(AgentInfo agent);

    }
}
