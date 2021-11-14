using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishingData
{
    public class PiStatusService : IPiStatusService
    {
        public PiStatusService()
        {

        }
        public PiStatus GetPiStatus()
        {
            return new PiStatus()
            {
                CpuHeat = 0,
                CpuUsage = 0,
                RamUsage = 0,
            };
        }
    }
}
