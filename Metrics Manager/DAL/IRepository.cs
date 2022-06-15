using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.DAL
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetByAgentId(int agentId, DateTimeOffset fromTime, DateTimeOffset toTime);
        IList<T> GetByCluster(DateTimeOffset fromTime, DateTimeOffset toTime);
        DateTimeOffset GetLastTime(int agentId);
        public void Create(T metric);
    }
}
