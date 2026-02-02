using Microsoft.UI.Xaml.Controls;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class SerialPortPage : Page
    {
        public SerialPortViewModel ViewModel { get; }

        public SerialPortPage()
        {
            InitializeComponent();
            ViewModel = (SerialPortViewModel)DataContext;
        }
    }
}
