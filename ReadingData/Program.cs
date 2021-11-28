using ConvertType;
using NModbus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Net.Http.Json;

namespace ReadingData
{
    internal class Program
    {
        private static readonly HttpClient _client = new HttpClient();
        static void Main(string[] args)
        {

            var modbusFactory = new ModbusFactory();
            var tcpClient = new TcpClient();
            IModbusMaster master = modbusFactory.CreateMaster(tcpClient);
            master.Transport.Retries = 3;
            master.Transport.WaitToRetryMilliseconds = 1000;
            master.Transport.SlaveBusyUsesRetryCount = true;
            var linuxServiceIp = ConfigurationManager.AppSettings["LinuxServiceIp"];
            var linuxServicePort = Convert.ToInt32(ConfigurationManager.AppSettings["LinuxServicePort"]);
            var dataStoreServiceUrl = ConfigurationManager.AppSettings["DataStoreUrl"];
            tcpClient.ConnectAsync(linuxServiceIp, linuxServicePort);
            while (true)
            {
                if (!tcpClient.Connected)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                var dataList = new List<float>();
                for (ushort i = 0; i < 3; i++)
                {
                    var result = master.ReadHoldingRegisters(0, (ushort)(18000 + i), 2);
                    dataList.Add(result.ToFloat());
                }
                var sysPerformData = new PiStatusData()
                {
                    CpuUsage = (float)Math.Round(dataList[0],2),
                    MemoryUsage = (float)Math.Round(dataList[1], 2),
                    CpuTemperature = (float)Math.Round(dataList[2], 2),
                    TimeStamp = DateTime.Now
                };

                _client.PostAsJsonAsync(dataStoreServiceUrl + "/deviceperformance", sysPerformData);
                Console.WriteLine($"Cpu Usage:{sysPerformData.CpuUsage} -- Cpu Temperature:{sysPerformData.CpuTemperature} -- Ram Usage:{sysPerformData.MemoryUsage} -- TimeStamp:{sysPerformData.TimeStamp}");
                Thread.Sleep(6000);
            }
        }

    }
}
