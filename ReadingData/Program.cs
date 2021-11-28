using ConvertType;
using NModbus;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace ReadingData
{
    internal class Program
    {
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
                    CpuUsage = dataList[0],
                    MemoryUsage = dataList[1],
                    CpuTemperature = dataList[2],
                    TimeStamp = DateTime.Now
                };
                Console.WriteLine($"Cpu Usage:{sysPerformData.CpuUsage} -- Cpu Temperature:{sysPerformData.CpuTemperature} -- Ram Usage:{sysPerformData.MemoryUsage} -- TimeStamp:{sysPerformData.TimeStamp}");
                Thread.Sleep(6000);
            }
        }

    }
}
