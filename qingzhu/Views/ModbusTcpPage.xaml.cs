using Microsoft.UI.Xaml.Controls;
using qingzhu.Services;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class ModbusTcpPage : Page
    {
        public ModbusTcpViewModel ViewModel => ViewModelLocator.Instance.ModbusTcpViewModel;

        public ModbusTcpPage()
        {
            InitializeComponent();
        }
    }
}
