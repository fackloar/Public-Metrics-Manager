using AutoMapper;
using MetricsManager.DAL;

namespace MetricsManager
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // добавлять сопоставления в таком стиле нужно для всех объектов 
            CreateMap<CpuMetric, DAL.Responses.AllCpuMetricsApiResponse.CpuMetricDto>();
            CreateMap<DotNetMetric, DAL.Responses.AllDotNetMetricsApiResponse.DotNetMetricDto>();
            CreateMap<HddMetric, DAL.Responses.AllHddMetricsApiResponse.HddMetricDto>();
            CreateMap<NetworkMetric, DAL.Responses.AllNetworkMetricsApiResponse.NetworkMetricDto>();
            CreateMap<RamMetric, DAL.Responses.AllRamMetricsApiResponse.RamMetricDto>();
        }
    }
}
