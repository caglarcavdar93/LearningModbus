using StoringData.DevicePerformanceServices.Dto;

namespace StoringData.DevicePerformanceServices
{
    public interface IDevicePerformanceService
    {
        public Task<DevicePerformance> CreateDevicePerformance(CreateDevicePerformance devicePerformance);
    }
}
