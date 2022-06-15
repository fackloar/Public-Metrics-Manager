using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using Dapper;
using MetricsManager.DAL;
using static MetricsManager.DAL.Responses.AllAgentsResponse;

namespace MetricsManager.DAL.Repositories
{
    public class AgentsRepository : IAgentsRepository
    {

        private readonly ILogger<AgentsRepository> _logger;
        private const string ConnectionString = ConnectionStringClass.ConnectionString;
        public AgentsRepository(ILogger<AgentsRepository> logger)
        {
            _logger = logger;
        }

        public string GetAddressByAgentId(int agentId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var response = connection.QuerySingle<string>("SELECT AgentUrl FROM agents WHERE AgentId=@AgentId",
                    new
                    {
                        AgentId = agentId
                    }).ToList();

                return response.ToString();
            }
        }

        public IList<AgentInfo> GetAgents()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var response = connection.Query<AgentInfo>("SELECT AgentId, AgentUrl FROM agents").ToList();
                return response;
            }
        }

        public void Register(AgentInfo agent)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO agents(AgentUrl) VALUES(@AgentUrl)",
                    new
                    {
                        AgentUrl = agent.AgentUrl
                    });
            }
        }
    }
}
