using Microsoft.UI.Xaml.Controls;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class TcpIpPage : Page
    {
        public TcpIpViewModel ViewModel { get; }

        public TcpIpPage()
        {
            InitializeComponent();
            ViewModel = (TcpIpViewModel)DataContext;
        }
    }
}
