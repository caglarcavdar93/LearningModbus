using System.Linq;
using Iot.Device.CpuTemperature;

namespace PublishingData.DevicePerformanceInfo
{
    public class PiDevicePerformanceInfo : IPiDevicePerformanceInfo
    {
        private readonly CpuTemperature _cpuTemperature;
        public PiDevicePerformanceInfo()
        {
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
            return 0;
        }

        private ushort GetCpuUsage()
        {
            return 0;
        }

        private ushort GetCpuHeat()
        {
            var heat= _cpuTemperature.Temperature.DegreesCelsius;
            return (ushort)heat;
        }
    }
}
