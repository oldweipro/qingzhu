using Microsoft.UI.Xaml.Controls;
using qingzhu.Services;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class ModbusPage : Page
    {
        public ModbusViewModel ViewModel => ViewModelLocator.Instance.ModbusViewModel;

        public ModbusPage()
        {
            InitializeComponent();
        }
    }
}
