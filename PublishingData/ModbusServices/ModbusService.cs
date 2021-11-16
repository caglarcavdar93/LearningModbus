using NModbus;
using NModbus.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PublishingData
{
    public class ModbusService : IModbusService
    {
        private IModbusSlaveNetwork _slaveNetwork;
        private IModbusMaster _master;
        private readonly IModbusFactory _modbusFactory;
        private TcpClient _tcpClient;
        private readonly IPAddress _ipAddress;
        private readonly int _port;
        public ModbusService(IModbusFactory modbusFactory)
        {
            _modbusFactory = modbusFactory;
            var host = Dns.GetHostEntry(Dns.GetHostName());
            _ipAddress = host.AddressList.Where(x => x.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault()?? IPAddress.Parse("127.0.0.1");
            _port = 10512;
        }
        public async void CreateServer()
        {
            TcpListener tcpListener = new TcpListener(_ipAddress, _port);
            _slaveNetwork = _modbusFactory.CreateSlaveNetwork(tcpListener);
            tcpListener.Start();
            var slave = _modbusFactory.CreateSlave(1);
            _slaveNetwork.AddSlave(slave);
            await _slaveNetwork.ListenAsync();
        }
        public void CreatePublisher()
        {
            _tcpClient = new TcpClient();
            _master = _modbusFactory.CreateMaster(_tcpClient);
            _tcpClient.Connect(_ipAddress, _port);
        }
        public void Dispose()
        {
            _master.Dispose();
            _slaveNetwork.Dispose();
            _tcpClient?.Dispose();
        }

        public void Publish<T>(T data) where T : class
        {
            if (_tcpClient.Connected)
            {
                var props = typeof(T).GetProperties();
                ushort i = 10800;
                ushort[] dataValues = new ushort[props.Length];
                for (var j = 0; j < props.Length; j++)
                {
                    var value = Convert.ToUInt16(props[j].GetValue(data));
                    dataValues[j] = value;
                }
                _master.WriteMultipleRegisters(1, i, dataValues);
            }

        }
    }
}
