using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishingData
{
    public class PiStatus
    {
        public ushort CpuUsage { get; set; }
        public ushort CpuHeat { get; set; }
        public ushort RamUsage { get; set; }
    }
}
