﻿using NModbus;
using System.Net;
using System.Net.Sockets;

namespace PublishingData.ModbusServices
{
    public class ModbusServer : IModbusServer
    {
        private readonly IModbusFactory _modbusFactory;
        private IModbusSlaveNetwork _slaveNetwork;
        private TcpListener _tcpListener;
        public ModbusServer()
        {
            _modbusFactory = new ModbusFactory();
        }

        public void CreateServer(IPAddress ip,int port)
        {
            _tcpListener = new TcpListener(ip, port);
            _tcpListener.Start();
            _slaveNetwork = _modbusFactory.CreateSlaveNetwork(_tcpListener);
        }
        public void AddSlave(byte unitId)
        {
            var slave = _modbusFactory.CreateSlave(unitId);
            _slaveNetwork.AddSlave(slave);
        }
        public void StartServer()
        {
            _slaveNetwork.ListenAsync();
        }

        public void Dispose()
        {
            _slaveNetwork?.Dispose();
            _slaveNetwork = null;
            _tcpListener?.Stop();
            _tcpListener = null;
        }
    }
}
