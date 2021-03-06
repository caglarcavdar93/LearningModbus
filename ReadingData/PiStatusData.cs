using System;

namespace ReadingData
{
    public class PiStatusData
    {
        public float CpuUsage { get; set; }
        public float MemoryUsage { get; set; }
        public float CpuTemperature { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
