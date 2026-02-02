using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace qingzhu.ViewModels
{
    public partial class TcpIpViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string mode = "Server";

        [ObservableProperty]
        private string ipAddress = "127.0.0.1";

        [ObservableProperty]
        private int port = 8080;

        [ObservableProperty]
        private bool isConnected = false;

        [ObservableProperty]
        private string status = "未连接";

        [ObservableProperty]
        private string dataMonitorText = string.Empty;

        [ObservableProperty]
        private string sendDataText = string.Empty;

        public TcpIpViewModel()
        {
        }

        [RelayCommand]
        private void StartServer()
        {
            IsConnected = true;
            Status = "运行中";
        }

        [RelayCommand]
        private void StopServer()
        {
            IsConnected = false;
            Status = "未连接";
        }

        [RelayCommand]
        private void SendData()
        {
            // Send TCP/IP data
        }

        [RelayCommand]
        private void ClearMonitor()
        {
            DataMonitorText = string.Empty;
        }
    }
}
