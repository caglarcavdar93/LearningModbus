using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishingData
{
    public interface IModbusService : IDisposable
    {
        public void CreateServer();
        public void CreatePublisher();
        public void Publish<T>(T data) where T : class;

    }
}
