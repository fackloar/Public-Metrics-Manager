using AutoMapper;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using System;
using static MetricsAgent.Responses.AllCpuMetricsResponse;
using static MetricsAgent.Responses.AllDotNetMetricsResponse;
using static MetricsAgent.Responses.AllHddMetricsResponse;
using static MetricsAgent.Responses.AllNetworkMetricsResponse;
using static MetricsAgent.Responses.AllRamMetricsResponse;

namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // добавлять сопоставления в таком стиле нужно для всех объектов 
            CreateMap<long, DateTimeOffset>().ConvertUsing(new DtoConverter());
            CreateMap<DateTimeOffset, long>().ConvertUsing(new LongConverter());
            CreateMap<CpuMetric, CpuMetricDto>();
            CreateMap<DotNetMetric, DotNetMetricDto>();
            CreateMap<HddMetric, HddMetricDto>();
            CreateMap<NetworkMetric, NetworkMetricDto>();
            CreateMap<RamMetric, RamMetricDto>();
        }
    }
}
