using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PublishingData.DevicePerformanceInfo;
using PublishingData.ModbusServices;

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
                    services.AddTransient<IPiDevicePerformanceInfo, PiDevicePerformanceInfo>();
                });
    }
}
