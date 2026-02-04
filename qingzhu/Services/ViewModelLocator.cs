using qingzhu.ViewModels;

namespace qingzhu.Services
{
    public class ViewModelLocator
    {
        private static ViewModelLocator? _instance;
        public static ViewModelLocator Instance => _instance ??= new ViewModelLocator();

        public ModbusTcpViewModel ModbusTcpViewModel { get; }
        public ModbusViewModel ModbusViewModel { get; }
        public ModbusTcpClientViewModel ModbusTcpClientViewModel { get; }

        private ViewModelLocator()
        {
            ModbusTcpViewModel = new ModbusTcpViewModel();
            ModbusViewModel = new ModbusViewModel();
            ModbusTcpClientViewModel = new ModbusTcpClientViewModel();
        }
    }
}
