using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using Dapper;

namespace MetricsManager.DAL.Repositories
{
    public interface ICpuMetricsRepository : IRepository<CpuMetric>
    {

    }
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private readonly ILogger<CpuMetricsRepository> _logger;

        private const string ConnectionString = ConnectionStringClass.ConnectionString;
        public CpuMetricsRepository(ILogger<CpuMetricsRepository> logger)
        {
            _logger = logger;
        }

        public IList<CpuMetric> GetByAgentId(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetric>("SELECT * FROM cpumetrics WHERE (AgentId=@agentId) and ((time>=@fromTime) AND (time<=@toTime))",
                    new
                    {
                        fromTime = fromTime.ToUnixTimeSeconds(),
                        toTime = toTime.ToUnixTimeSeconds(),
                        agentId = agentId
                    }).ToList();
            }
        }
        public IList<CpuMetric> GetByCluster(DateTimeOffset fromTime, DateTimeOffset toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetric>("SELECT * FROM cpumetrics WHERE (time>=@fromTime) AND (time<=@toTime)",
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
                var getTimeByAgent = connection.QueryFirstOrDefault<DateTimeOffset>("SELECT time FROM cpumetrics WHERE (AgentId=@agentId) ORDER BY Id DESC",
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
        public void Create(CpuMetric metric)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO cpumetrics(value, time, agentId) VALUES(@value, @time, @agentId)",
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
