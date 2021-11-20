using System;
using System.Net;

namespace PublishingData.ModbusServices
{
    public interface IModbusClient : IDisposable
    {
        public void CreateMaster(IPAddress ip, int port);
        public void WriteData(byte unitId, ushort address, ushort data);
    }
}
