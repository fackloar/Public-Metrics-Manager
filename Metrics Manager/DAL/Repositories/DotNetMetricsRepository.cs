using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using Dapper;

namespace MetricsManager.DAL.Repositories
{
    public interface IDotNetMetricsRepository : IRepository<DotNetMetric>
    {

    }
    public class DotNetMetricsRepository : IDotNetMetricsRepository
    {
        private readonly ILogger<DotNetMetricsRepository> _logger;

        private const string ConnectionString = ConnectionStringClass.ConnectionString;
        public DotNetMetricsRepository(ILogger<DotNetMetricsRepository> logger)
        {
            _logger = logger;
        }

        public IList<DotNetMetric> GetByAgentId(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<DotNetMetric>("SELECT * FROM DotNetmetrics WHERE (AgentId=@agentId) and ((time>=@fromTime) AND (time<=@toTime))",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds(),
                        agentId = agentId
                    }).ToList();
            }
        }
        public IList<DotNetMetric> GetByCluster(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<DotNetMetric>("SELECT * FROM DotNetmetrics WHERE (time>=@fromTime) AND (time<=@toTime)",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds(),
                    }).ToList();
            }
        }
        public DateTimeOffset GetLastTime(int agentId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                var getTimeByAgent = connection.QueryFirstOrDefault<DateTimeOffset>("SELECT time FROM DotNetmetrics WHERE (AgentId=@agentId) ORDER BY Id DESC",
                    new
                    {
                        agentId = agentId
                    });
                if (getTimeByAgent >= DateTimeOffset.MinValue)
                {
                    return getTimeByAgent;
                }
                else
                {
                    return DateTimeOffset.UnixEpoch;
                }
            }
        }
        public void Create(DotNetMetric metric)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO DotNetmetrics(value, time, agentId) VALUES(@value, @time, @agentId)",
                    new
                    {
                        value = metric.Value,
                        time = metric.Time,
                        agentId = metric.AgentId
                    });
            }
        }
    }
}
