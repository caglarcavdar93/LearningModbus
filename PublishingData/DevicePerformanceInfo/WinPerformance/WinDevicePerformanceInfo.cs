using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PublishingData.DevicePerformanceInfo
{
    public class WinDevicePerformanceInfo : IWinDevicePerformanceInfo
    {
        private readonly Computer _computer;
        public WinDevicePerformanceInfo()
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
            var performanceCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use", null, true);
            var val = performanceCounter.NextValue();
            return (ushort)val;
        }

        private ushort GetCpuUsage()
        {
            var performanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            var val = performanceCounter.NextValue();
            Thread.Sleep(1000);
            val = performanceCounter.NextValue();

            return (ushort)val;
        }
        private ushort GetCpuHeat()
        {
           
            var hardwareItems = _computer.Hardware.Where(x=>x.HardwareType== HardwareType.Cpu).FirstOrDefault();
            hardwareItems.Update();
            var sensor = hardwareItems.Sensors.Where(x=> x.SensorType == SensorType.Temperature).FirstOrDefault();

            return (ushort)sensor.Value.Value;
        }
    }
}
