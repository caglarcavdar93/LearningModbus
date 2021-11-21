using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishingData.DevicePerformanceInfo
{
    public class DevicePerformance
    {
        public ushort CpuUsage { get; set; }
        public ushort MemoryUsage { get; set; }
        public ushort CpuHeat { get; set; }
    }
}
