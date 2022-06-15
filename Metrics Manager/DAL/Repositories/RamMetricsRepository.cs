using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using Dapper;

namespace MetricsManager.DAL.Repositories
{
    public interface IRamMetricsRepository : IRepository<RamMetric>
    {

    }
    public class RamMetricsRepository : IRamMetricsRepository
    {
        private readonly ILogger<RamMetricsRepository> _logger;

        private const string ConnectionString = ConnectionStringClass.ConnectionString;
        public RamMetricsRepository(ILogger<RamMetricsRepository> logger)
        {
            _logger = logger;
        }

        public IList<RamMetric> GetByAgentId(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<RamMetric>("SELECT * FROM Rammetrics WHERE (AgentId=@agentId) and ((time>=@fromTime) AND (time<=@toTime))",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds(),
                        agentId = agentId
                    }).ToList();
            }
        }
        public IList<RamMetric> GetByCluster(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<RamMetric>("SELECT * FROM Rammetrics WHERE (time>=@fromTime) AND (time<=@toTime)",
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
                var getTimeByAgent = connection.QueryFirstOrDefault<DateTimeOffset>("SELECT time FROM Rammetrics WHERE (AgentId=@agentId) ORDER BY Id DESC",
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
        public void Create(RamMetric metric)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO Rammetrics(value, time, agentId) VALUES(@value, @time, @agentId)",
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
