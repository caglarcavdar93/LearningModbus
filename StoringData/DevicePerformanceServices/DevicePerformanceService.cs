﻿using StoringData.Ctx;
using StoringData.DevicePerformanceServices.Dto;

namespace StoringData.DevicePerformanceServices
{
    public class DevicePerformanceService : IDevicePerformanceService
    {
        private readonly DevicePerformanceDbContext _ctx;
        public DevicePerformanceService(DevicePerformanceDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<DevicePerformance> CreateDevicePerformance(CreateDevicePerformance devicePerformance)
        {
            DevicePerformance entity = new DevicePerformance()
            {
                CpuTemperature = devicePerformance.CpuTemperature,
                CpuUsage = devicePerformance.CpuUsage,
                MemoryUsage = devicePerformance.MemoryUsage,
                TimeStamp = devicePerformance.TimeStamp
            };
            _ctx.DevicePerformances.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity;
        }
    }
}
