using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using qingzhu.Views;
using qingzhu.ViewModels;

namespace qingzhu
{
    public sealed partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            
            ViewModel = new MainViewModel();
            
            Title = "工业协议模拟器";
            ExtendsContentIntoTitleBar = true;
            
            // 仅将自定义标题栏区域设置为拖拽区域，而不是整个NavigationView
            SetTitleBar(AppTitleBar);
            
            // Navigate to home page by default
            ContentFrame.Navigate(typeof(HomePage));
            NavView.SelectedItem = NavView.MenuItems.OfType<NavigationViewItem>().First();
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(SettingsPage));
            }
            else if (args.SelectedItemContainer != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag?.ToString();
                if (navItemTag != null)
                {
                    NavigateToPage(navItemTag);
                }
            }
        }

        private void NavigateToPage(string navItemTag)
        {
            Type? pageType = navItemTag switch
            {
                "Home" => typeof(HomePage),
                "SerialPort" => typeof(SerialPortPage),
                "Modbus" => typeof(ModbusPage),
                "ModbusTCP" => typeof(ModbusTcpPage),
                "TCPIP" => typeof(TcpIpPage),
                "HTTP" => typeof(HttpPage),
                "MQTT" => typeof(MqttPage),
                "OPC" => typeof(OpcUaPage),
                _ => null
            };

            if (pageType != null && ContentFrame.CurrentSourcePageType != pageType)
            {
                ContentFrame.Navigate(pageType);
            }
        }
    }
}
