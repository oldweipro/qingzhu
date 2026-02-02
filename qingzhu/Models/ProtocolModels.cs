namespace qingzhu.Models
{
    public class SerialPortConfig
    {
        public string? PortName { get; set; }
        public int BaudRate { get; set; } = 9600;
        public int DataBits { get; set; } = 8;
        public string Parity { get; set; } = "None";
        public string StopBits { get; set; } = "1";
    }

    public class ModbusConfig
    {
        public int SlaveId { get; set; } = 1;
        public string? PortName { get; set; }
        public int BaudRate { get; set; } = 9600;
    }

    public class OpcUaNodeData
    {
        public string? NodeId { get; set; }
        public string? DisplayName { get; set; }
        public string? DataType { get; set; }
        public object? Value { get; set; }
    }
}
