using Microsoft.UI.Xaml.Controls;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class OpcUaPage : Page
    {
        public OpcUaViewModel ViewModel { get; }

        public OpcUaPage()
        {
            InitializeComponent();
            ViewModel = (OpcUaViewModel)DataContext;
        }
    }
}
