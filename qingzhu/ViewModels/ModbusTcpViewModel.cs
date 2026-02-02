using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace qingzhu.ViewModels
{
    public partial class ModbusTcpViewModel : ViewModelBase
    {
        [ObservableProperty]
        private int slaveId = 1;

        [ObservableProperty]
        private string ipAddress = "127.0.0.1";

        [ObservableProperty]
        private int port = 502;

        [ObservableProperty]
        private bool isRunning = false;

        [ObservableProperty]
        private string status = "已停止";

        [ObservableProperty]
        private int connectionCount = 0;

        [ObservableProperty]
        private int requestCount = 0;

        [ObservableProperty]
        private int errorCount = 0;

        public ModbusTcpViewModel()
        {
        }

        [RelayCommand]
        private void StartServer()
        {
            IsRunning = true;
            Status = "运行中";
        }

        [RelayCommand]
        private void StopServer()
        {
            IsRunning = false;
            Status = "已停止";
            ConnectionCount = 0;
            RequestCount = 0;
            ErrorCount = 0;
        }
    }
}
