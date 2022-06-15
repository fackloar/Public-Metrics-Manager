using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MetricsAgent
{
    public class LongConverter : ITypeConverter<DateTimeOffset, long>
    {
        public long Convert(DateTimeOffset source, long destination, ResolutionContext context)
        {
            return source.ToUnixTimeSeconds();
        }
    }
}