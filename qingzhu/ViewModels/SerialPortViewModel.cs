using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace qingzhu.ViewModels
{
    public partial class SerialPortViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? selectedPort;

        [ObservableProperty]
        private int baudRate = 38400;

        [ObservableProperty]
        private int dataBits = 8;

        [ObservableProperty]
        private string parity = "None";

        [ObservableProperty]
        private string stopBits = "1";

        [ObservableProperty]
        private bool isConnected = false;

        [ObservableProperty]
        private string connectionStatus = "未连接";

        [ObservableProperty]
        private int rxBytes = 0;

        [ObservableProperty]
        private int txBytes = 0;

        [ObservableProperty]
        private string dataMonitorText = string.Empty;

        [ObservableProperty]
        private string sendDataText = string.Empty;

        [ObservableProperty]
        private string dataFormat = "HEX";

        [ObservableProperty]
        private bool autoAddNewLine = false;

        public ObservableCollection<string> AvailablePorts { get; } = new()
        {
            "COM1", "COM2", "COM3", "COM4"
        };

        public ObservableCollection<int> BaudRates { get; } = new()
        {
            4800, 9600, 19200, 38400, 57600, 115200
        };

        public SerialPortViewModel()
        {
        }

        [RelayCommand]
        private void OpenPort()
        {
            IsConnected = true;
            ConnectionStatus = "已连接";
        }

        [RelayCommand]
        private void ClosePort()
        {
            IsConnected = false;
            ConnectionStatus = "未连接";
        }

        [RelayCommand]
        private void SendData()
        {
            if (IsConnected && !string.IsNullOrEmpty(SendDataText))
            {
                TxBytes += SendDataText.Length;
            }
        }

        [RelayCommand]
        private void ClearMonitor()
        {
            DataMonitorText = string.Empty;
        }

        [RelayCommand]
        private void ExportData()
        {
            // Export data to file
        }
    }
}
