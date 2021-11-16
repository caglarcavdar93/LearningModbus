using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NModbus;
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
                    services.AddTransient<IModbusService, ModbusService>();
                    services.AddTransient<IPiStatusService, PiStatusService>();
                    services.AddTransient<IModbusFactory, ModbusFactory>();
                    services.AddHostedService<Worker>();
                });
    }
}
