using LibreHardwareMonitor.Hardware;
using System.Linq;

namespace PublishingData.DevicePerformanceInfo
{
    public class PiDevicePerformanceInfo : IPiDevicePerformanceInfo
    {
        private readonly Computer _computer;
        public PiDevicePerformanceInfo()
        {
            _computer = new Computer()
            {
                IsCpuEnabled = true
            };
            _computer.Open();
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

            var hardwareItems = _computer.Hardware.Where(x => x.HardwareType == HardwareType.Cpu).FirstOrDefault();
            hardwareItems.Update();
            var sensor = hardwareItems.Sensors.Where(x => x.SensorType == SensorType.Temperature).FirstOrDefault();

            return (ushort)sensor.Value.Value;
        }
    }
}
