using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NModbus;
using PublishingData.DevicePerformanceInfo;
using PublishingData.ModbusServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishingData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddTransient<IModbusServer, ModbusServer>();
                    services.AddTransient<IModbusClient, ModbusClient>();
                    services.AddTransient<IMacDevicePerformanceInfo, MacDevicePerformanceInfo>();
                    services.AddTransient<IPiDevicePerformanceInfo, PiDevicePerformanceInfo>();
                    services.AddTransient<IWinDevicePerformanceInfo, WinDevicePerformanceInfo>();
                });
    }
}
