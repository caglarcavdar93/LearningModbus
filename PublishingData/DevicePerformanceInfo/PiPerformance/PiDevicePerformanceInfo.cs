using System.Linq;
using Iot.Device.CpuTemperature;

namespace PublishingData.DevicePerformanceInfo
{
    public class PiDevicePerformanceInfo : IPiDevicePerformanceInfo
    {
        private readonly CpuTemperature _cpuTemperature;
        public PiDevicePerformanceInfo()
        {
            //vcgencmd gives all the info
            _cpuTemperature = new CpuTemperature();
        }
        public DevicePerformance GetPerformanceInfo()
        {
            return new DevicePerformance()
            {
                CpuUsage = GetCpuUsage(),
                MemoryUsage = GetMemoryUsage(),
                CpuHeat = GetCpuHeat(),
            };
        }

        private ushort GetMemoryUsage()
        {
            //Memory info is in /proc/meminfo
            return 0;
        }

        private ushort GetCpuUsage()
        {
            //Cpu info is in /proc/stat 
            return 0;
        }

        private ushort GetCpuHeat()
        {
            double heat = 0;
            if (_cpuTemperature.IsAvailable)
                heat = _cpuTemperature.Temperature.DegreesCelsius;
            return (ushort)heat;
        }
    }
}
