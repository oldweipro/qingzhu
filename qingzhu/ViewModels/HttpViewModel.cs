using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace qingzhu.ViewModels
{
    public partial class HttpViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string baseUrl = "http://localhost:5000";

        [ObservableProperty]
        private int port = 5000;

        [ObservableProperty]
        private bool isRunning = false;

        [ObservableProperty]
        private string status = "已停止";

        [ObservableProperty]
        private int requestCount = 0;

        public HttpViewModel()
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
            RequestCount = 0;
        }

        [RelayCommand]
        private void AddEndpoint()
        {
            // Add new endpoint
        }

        [RelayCommand]
        private void RemoveEndpoint()
        {
            // Remove endpoint
        }
    }
}
