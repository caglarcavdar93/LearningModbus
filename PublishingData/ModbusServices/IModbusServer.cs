using System;
using System.Net;

namespace PublishingData.ModbusServices
{
    public interface IModbusServer : IDisposable
    {
        public void CreateServer(IPAddress ip, int port);
        public void AddSlave(byte unitId);
        public void StartServer();
    }
}
