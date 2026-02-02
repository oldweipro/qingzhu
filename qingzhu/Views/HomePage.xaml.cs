using Microsoft.UI.Xaml.Controls;
using qingzhu.ViewModels;

namespace qingzhu.Views
{
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel { get; }

        public HomePage()
        {
            InitializeComponent();
            ViewModel = (HomeViewModel)DataContext;
        }
    }
}
