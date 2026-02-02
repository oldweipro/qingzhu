using Microsoft.UI.Xaml.Controls;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class ModbusPage : Page
    {
        public ModbusViewModel ViewModel { get; }

        public ModbusPage()
        {
            InitializeComponent();
            ViewModel = (ModbusViewModel)DataContext;
        }
    }
}
