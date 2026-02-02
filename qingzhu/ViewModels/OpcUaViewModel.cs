using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace qingzhu.ViewModels
{
    public partial class OpcUaViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string endpointUrl = "opc.tcp://localhost:4840";

        [ObservableProperty]
        private string serverName = "OPC UA Simulator";

        [ObservableProperty]
        private int port = 4840;

        [ObservableProperty]
        private string securityPolicy = "None";

        [ObservableProperty]
        private string securityMode = "None";

        [ObservableProperty]
        private bool anonymousAccess = true;

        [ObservableProperty]
        private bool usernamePasswordAuth = false;

        [ObservableProperty]
        private bool isServerRunning = false;

        [ObservableProperty]
        private string serverStatus = "已停止";

        [ObservableProperty]
        private int sessionCount = 0;

        [ObservableProperty]
        private int subscriptionCount = 0;

        public ObservableCollection<string> SecurityPolicies { get; } = new()
        {
            "None",
            "Basic128Rsa15",
            "Basic256",
            "Basic256Sha256"
        };

        public ObservableCollection<string> SecurityModes { get; } = new()
        {
            "None",
            "Sign",
            "SignAndEncrypt"
        };

        public OpcUaViewModel()
        {
        }

        [RelayCommand]
        private void StartServer()
        {
            IsServerRunning = true;
            ServerStatus = "运行中";
        }

        [RelayCommand]
        private void StopServer()
        {
            IsServerRunning = false;
            ServerStatus = "已停止";
            SessionCount = 0;
            SubscriptionCount = 0;
        }

        [RelayCommand]
        private void AddNode()
        {
            // Add new OPC UA node
        }

        [RelayCommand]
        private void ImportNodes()
        {
            // Import nodes from file
        }

        [RelayCommand]
        private void ExportNodes()
        {
            // Export nodes to file
        }
    }
}
