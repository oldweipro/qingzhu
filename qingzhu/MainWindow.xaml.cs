using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using qingzhu.ViewModels;
using qingzhu.Views;

namespace qingzhu;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ViewModel = new MainViewModel();
        Title = "青竹工业协议模拟器";
        ExtendsContentIntoTitleBar = true;

        // 让 WinUI 自动处理标题栏
        // SetTitleBar(AppTitleBar);

        // Navigate to home page by default
        ContentFrame.Navigate(typeof(HomePage));
        NavView.SelectedItem = NavView.MenuItems.OfType<NavigationViewItem>().First();
    }

    public MainViewModel ViewModel { get; }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            ContentFrame.Navigate(typeof(SettingsPage));
        }
        else if (args.SelectedItemContainer != null)
        {
            var navItemTag = args.SelectedItemContainer.Tag?.ToString();
            if (navItemTag != null) NavigateToPage(navItemTag);
        }
    }

    private void NavigateToPage(string navItemTag)
    {
        var pageType = navItemTag switch
        {
            "Home" => typeof(HomePage),
            "SerialPort" => typeof(SerialPortPage),
            "Modbus" => typeof(ModbusPage),
            "ModbusTCP" => typeof(ModbusTcpPage),
            "ModbusTCPClient" => typeof(ModbusTcpClientPage),
            "TCPIP" => typeof(TcpIpPage),
            "HTTP" => typeof(HttpPage),
            "MQTT" => typeof(MqttPage),
            "OPC" => typeof(OpcUaPage),
            _ => null
        };

        if (pageType != null && ContentFrame.CurrentSourcePageType != pageType) ContentFrame.Navigate(pageType);
    }
}