﻿using ConvertType;
using NModbus;
using System;
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
            tcpClient.ConnectAsync("127.0.0.1", 10502);
            while (true)
            {
                Thread.Sleep(6000);
                var result = master.ReadHoldingRegisters(0, 18000, 2);
                var floatResult = result.ToFloat();
                Console.WriteLine(floatResult);
            }
        }
    }
}
