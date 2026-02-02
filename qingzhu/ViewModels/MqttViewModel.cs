using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace qingzhu.ViewModels
{
    public partial class MqttViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string brokerAddress = "localhost";

        [ObservableProperty]
        private int port = 1883;

        [ObservableProperty]
        private string clientId = "QingzhuSimulator";

        [ObservableProperty]
        private bool isConnected = false;

        [ObservableProperty]
        private string status = "未连接";

        [ObservableProperty]
        private int messageCount = 0;

        [ObservableProperty]
        private string topic = string.Empty;

        [ObservableProperty]
        private string message = string.Empty;

        public MqttViewModel()
        {
        }

        [RelayCommand]
        private void Connect()
        {
            IsConnected = true;
            Status = "已连接";
        }

        [RelayCommand]
        private void Disconnect()
        {
            IsConnected = false;
            Status = "未连接";
        }

        [RelayCommand]
        private void Subscribe()
        {
            // Subscribe to topic
        }

        [RelayCommand]
        private void Publish()
        {
            if (IsConnected)
            {
                MessageCount++;
            }
        }
    }
}
