using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PublishingData.DevicePerformanceInfo
{
    public class DevicePerformanceInfoFactory
    {
        public static IDevicePerformanceInfo Create()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return new MacDevicePerformanceInfo();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new PiDevicePerformanceInfo();
            }
            else
            {
                return new WinDevicePerformanceInfo();
            }
        }
    }
}
