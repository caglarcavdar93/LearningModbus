using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishingData.DevicePerformanceInfo
{
    public class DevicePerformance
    {
        public int CpuUsage { get; set; }
        public int DiskUsage { get; set; }
        public string DeviceName { get; set; }
        public int MemoryUsage { get; set; }
        public int CpuHeat { get; set; }
    }
}
