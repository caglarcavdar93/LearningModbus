using NModbus;
using System.Net;
using System.Net.Sockets;

namespace PublishingData.ModbusServices
{
    public class ModbusClient : IModbusClient
    {
        private readonly IModbusFactory _modbusFactory;
        private IModbusMaster _master;
        private TcpClient _tcpClient;
        public ModbusClient()
        {
            _modbusFactory = new ModbusFactory();
        }

        public void CreateMaster(IPAddress ip, int port)
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect(ip, port);
            _master = _modbusFactory.CreateMaster(_tcpClient);
        }

        public void Dispose()
        {
            _master?.Dispose();
            _master = null;
            _tcpClient?.Close();
            _tcpClient=null;
        }

        public void WriteData(byte unitId, ushort address, ushort data)
        {
            _master.WriteSingleRegister(unitId, address, data);
        }
    }
}
