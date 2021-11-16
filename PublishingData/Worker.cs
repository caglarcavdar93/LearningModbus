using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PublishingData
{
    public class Worker : BackgroundService
    {
        private readonly IModbusService _modbusService;
        private readonly ILogger<Worker> _logger;
        private readonly IPiStatusService _piStatusService;
        public Worker(IModbusService modbusService, ILogger<Worker> logger, IPiStatusService piStatusService)
        {
            //Create modbus server
            _modbusService = modbusService;
            _logger = logger;
            _piStatusService = piStatusService;

        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _modbusService.CreateServer();
            _modbusService.CreatePublisher();
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _modbusService.Dispose();
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Read CPU usage, memory usage, cpu heat
                var currentPiStatus = new PiStatus()
                {
                    CpuHeat = 55,
                    CpuUsage = 30,
                    RamUsage = 48,
                };//_piStatusService.GetPiStatus();
                _logger.LogInformation($"Cpu Usage:{currentPiStatus.CpuUsage} -- Cpu Heat:{currentPiStatus.CpuHeat} -- Ram Usage:{currentPiStatus.RamUsage}");
                _modbusService.Publish(currentPiStatus);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
