using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PublishingData.DevicePerformanceInfo;
using PublishingData.ModbusServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PublishingData
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IModbusServer _modbusServer;
        private readonly IModbusClient _modbusClient;
        private readonly IDevicePerformanceInfo _devicePerformance;
        private ushort _address = 10800;
        private byte _unitId = 0;
        public Worker(ILogger<Worker> logger, IModbusServer modbusServer, IModbusClient modbusClient)
        {
            //Create modbus server
            _modbusClient = modbusClient;
            _modbusServer = modbusServer;
            _devicePerformance = DevicePerformanceInfoFactory.Create();
            _logger = logger;

        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress= host.AddressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault()?? IPAddress.Parse("127.0.0.1");
            var port = 10502;
            _modbusServer.CreateServer(ipAddress, port);
            _modbusServer.AddSlave(_unitId);
            _modbusServer.StartServer();
            _modbusClient.CreateMaster(ipAddress, port);
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _modbusClient.Dispose();
            _modbusServer.Dispose();
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               
                var performanceData = _devicePerformance.GetPerformanceInfo();
                _modbusClient.WriteData(_unitId, _address, performanceData.MemoryUsage);
                _logger.LogInformation($"Cpu Usage:{performanceData.CpuUsage} -- Cpu Temperature:{performanceData.CpuHeat} -- Ram Usage:{performanceData.MemoryUsage}");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
