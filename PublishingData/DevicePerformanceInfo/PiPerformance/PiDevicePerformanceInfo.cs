using System;
using System.IO;
using System.Linq;
using System.Threading;
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
            var cpuUsage = ReadCpu();
            return (ushort)cpuUsage;
        }

        private ushort GetCpuHeat()
        {
            double heat = 0;
            if (_cpuTemperature.IsAvailable)
                heat = _cpuTemperature.Temperature.DegreesCelsius;
            return (ushort)heat;
        }

        private double ReadCpu()
        {
            var oldVal = "";
            var newVal = "";
            using (FileStream fileStream = new FileStream("/proc/stat", FileMode.Open, FileAccess.Read))
            {

                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    oldVal = streamReader.ReadLine();
                }
            }
            Thread.Sleep(1000);
            using (FileStream fileStream = new FileStream("/proc/stat", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    newVal = streamReader.ReadLine();
                }
            }
            var oldArr = oldVal.Split(' ').ToList();
            oldArr.RemoveRange(0, 2);
            var oldCpuValArr = oldArr.Select(x => Convert.ToDouble(x)).ToList();

            var newArr = newVal.Split(' ').ToList();
            newArr.RemoveRange(0, 2);
            var newCpuValArr = newArr.Select(x => Convert.ToDouble(x)).ToList();

            double prevIdle = oldCpuValArr[3] + oldCpuValArr[4];
            double idle = newCpuValArr[3] + newCpuValArr[4];

            var prevNonIdle = oldCpuValArr[0] + oldCpuValArr[1] + oldCpuValArr[2] + oldCpuValArr[5] + oldCpuValArr[6] + oldCpuValArr[7];
            double nonIdle = newCpuValArr[0] + newCpuValArr[1] + newCpuValArr[2] + newCpuValArr[5] + newCpuValArr[6] + newCpuValArr[7];

            var prevTotal = prevIdle + prevNonIdle;
            double total = idle + nonIdle;

            var totalDifference = total - prevTotal;
            var idleDifference = idle - prevIdle;

            double cpuPercentage = 0;


            cpuPercentage = (totalDifference - idleDifference) / (totalDifference);
            cpuPercentage = cpuPercentage * 100;
            return cpuPercentage;
        }
    }
}
