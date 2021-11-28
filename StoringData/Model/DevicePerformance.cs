namespace StoringData
{
    public class DevicePerformance
    {
        public int Id { get; set; }
        public float CpuUsage { get; set; }
        public float MemoryUsage { get; set; }
        public float CpuTemperature { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
