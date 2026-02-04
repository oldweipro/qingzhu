using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace qingzhu.ViewModels
{
    public partial class HomeViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string welcomeMessage = "欢迎使用青竹工业协议模拟器";

        [ObservableProperty]
        private string description = "支持串口、Modbus、TCP/IP、HTTP 等多种工业通信协议的模拟与测试";

        public HomeViewModel()
        {
        }

        [RelayCommand]
        private void NavigateToSerialPort()
        {
            // Navigation command
        }

        [RelayCommand]
        private void NavigateToModbus()
        {
            // Navigation command
        }

        [RelayCommand]
        private void NavigateToNetwork()
        {
            // Navigation command
        }
    }
}
