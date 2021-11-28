﻿using System;
using System.Collections.Generic;
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
                CpuTemperature = GetCpuTemperature(),
            };
        }

        private float GetMemoryUsage()
        {
            //Memory info is in /proc/meminfo
            var memoryInfoString = ReadMemory();
            var (memTotal, memAvailable) = ParseMemoryInfoString(memoryInfoString);
            float ramUsage = (float)Math.Round((double)(memTotal - memAvailable) * 100 /memTotal,2);
            return ramUsage;
        }

        private float GetCpuUsage()
        {
            //Cpu info is in /proc/stat 
            var cpuUsage = GetCpuUsagePercent();
            return cpuUsage;
        }

        private float GetCpuTemperature()
        {
            double temp = 0;
            if (_cpuTemperature.IsAvailable)
                temp = Math.Round(_cpuTemperature.Temperature.DegreesCelsius,2);
            return (float)temp;
        }

        private float GetCpuUsagePercent()
        {
            var oldVal = ReadCpu();
            Thread.Sleep(1000);
            var newVal = ReadCpu();

            var oldArr = ParseCpuString(oldVal);

            var newArr = ParseCpuString(newVal);

            return CalculateCpuUsage(oldArr, newArr);
        }
        private string ReadCpu()
        {
            using (FileStream fileStream = new FileStream("/proc/stat", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    return streamReader.ReadLine();
                }
            }
        }
        private List<double> ParseCpuString(string stringValues)
        {
            var splitted = stringValues.Split(' ').ToList();
            splitted.RemoveRange(0, 2);
            var cpuValArr = splitted.Select(x => Convert.ToDouble(x)).ToList();
            return cpuValArr;
        }

        private float CalculateCpuUsage(List<double> oldCpuValArr, List<double> newCpuValArr)
        {
            double prevIdle = oldCpuValArr[3] + oldCpuValArr[4];
            double idle = newCpuValArr[3] + newCpuValArr[4];

            var prevNonIdle = oldCpuValArr[0] + oldCpuValArr[1] + oldCpuValArr[2] + oldCpuValArr[5] + oldCpuValArr[6] + oldCpuValArr[7];
            double nonIdle = newCpuValArr[0] + newCpuValArr[1] + newCpuValArr[2] + newCpuValArr[5] + newCpuValArr[6] + newCpuValArr[7];

            var prevTotal = prevIdle + prevNonIdle;
            double total = idle + nonIdle;

            var totalDifference = total - prevTotal;
            var idleDifference = idle - prevIdle;

            float cpuPercentage = (float)(Math.Round((totalDifference - idleDifference) * 100 /totalDifference,2));
            return cpuPercentage;
        }

        private string ReadMemory()
        {
            using (FileStream fileStream = new FileStream("/proc/meminfo", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        private (uint memTotal, uint memAvailable) ParseMemoryInfoString(string memoryInfoString)
        {
            memoryInfoString = String.Concat(memoryInfoString.Where(c => !Char.IsWhiteSpace(c)));
            var infoLines = memoryInfoString.Split("kB").ToList();

            var memTotal = Convert.ToUInt32(infoLines[0].Split(':')[1]);
            var memAvailable = Convert.ToUInt32(infoLines[2].Split(':')[1]);

            return (memTotal, memAvailable);
        }
    }
}
