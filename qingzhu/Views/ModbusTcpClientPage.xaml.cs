using Microsoft.UI.Xaml.Controls;
using qingzhu.Services;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class ModbusTcpClientPage : Page
    {
        public ModbusTcpClientViewModel ViewModel => ViewModelLocator.Instance.ModbusTcpClientViewModel;

        public ModbusTcpClientPage()
        {
            InitializeComponent();
        }
    }
}
