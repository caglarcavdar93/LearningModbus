namespace StoringData.DevicePerformanceServices.Dto
{
    public record CreateDevicePerformance
    {
        public float CpuUsage { get; set; }
        public float MemoryUsage { get; set; }
        public float CpuTemperature { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
