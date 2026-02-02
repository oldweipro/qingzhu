using Microsoft.UI.Xaml.Controls;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class ModbusTcpPage : Page
    {
        public ModbusTcpViewModel ViewModel { get; }

        public ModbusTcpPage()
        {
            InitializeComponent();
            ViewModel = (ModbusTcpViewModel)DataContext;
        }
    }
}
