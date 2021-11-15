using NModbus;
using NModbus.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PublishingData
{
    public class ModbusService : IModbusService
    {
        private readonly IModbusMaster _master;
        private readonly TcpClient _tcpClient;
        public ModbusService()
        {
            var modbusFactory = new ModbusFactory();
            _tcpClient = new TcpClient();
            _master = modbusFactory.CreateMaster(_tcpClient);
        }
        public void CreateServer()
        {
            _tcpClient.Connect("localhost", 10510);
        }

        public void Dispose()
        {
            _tcpClient.Close();
            _tcpClient.Dispose();
            _master.Dispose();
        }

        public void Publish<T>(T data) where T : class
        {
            if (_tcpClient.Connected)
            {
                var props = typeof(T).GetProperties();
                ushort i = 10800;
                foreach (var prop in props)
                {
                    var value = Convert.ToUInt16(prop.GetValue(data));
                    _master.WriteSingleRegisterAsync(0, i, value);
                    i++;
                }
            }
        }
    }
}
