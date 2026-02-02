using Microsoft.UI.Xaml.Controls;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class MqttPage : Page
    {
        public MqttViewModel ViewModel { get; }

        public MqttPage()
        {
            InitializeComponent();
            ViewModel = (MqttViewModel)DataContext;
        }
    }
}
