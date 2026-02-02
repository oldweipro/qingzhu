using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace qingzhu.ViewModels
{
    public partial class ModbusViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int slaveId = 1;

        [ObservableProperty]
        private string? selectedPort;

        [ObservableProperty]
        private int baudRate = 9600;

        [ObservableProperty]
        private bool isRunning = false;

        [ObservableProperty]
        private string status = "已停止";

        [ObservableProperty]
        private int requestCount = 0;

        [ObservableProperty]
        private int errorCount = 0;

        public ObservableCollection<string> AvailablePorts { get; } = new()
        {
            "COM1", "COM2", "COM3", "COM4"
        };

        public ObservableCollection<int> BaudRates { get; } = new()
        {
            4800, 9600, 19200, 115200
        };

        public ModbusViewModel()
        {
        }

        [RelayCommand]
        private void StartSlave()
        {
            IsRunning = true;
            Status = "运行中";
        }

        [RelayCommand]
        private void StopSlave()
        {
            IsRunning = false;
            Status = "已停止";
            RequestCount = 0;
            ErrorCount = 0;
        }

        [RelayCommand]
        private void RandomFill()
        {
            // Fill registers with random data
        }

        [RelayCommand]
        private void ClearRegisters()
        {
            // Clear all registers
        }

        [RelayCommand]
        private void ImportData()
        {
            // Import register data
        }

        [RelayCommand]
        private void ExportData()
        {
            // Export register data
        }
    }
}
